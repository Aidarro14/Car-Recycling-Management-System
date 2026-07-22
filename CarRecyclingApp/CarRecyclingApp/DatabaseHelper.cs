using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Windows.Forms;

public class DatabaseHelper
{
    private readonly string connectionString;

    public DatabaseHelper()
    {
        var connectionStringSetting = ConfigurationManager.ConnectionStrings["MySqlConnection"];
        if (connectionStringSetting != null)
        {
            connectionString = connectionStringSetting.ConnectionString;
        }
        else
        {
            throw new InvalidOperationException("Строка подключения не найдена.");
        }
    }

    public bool TestConnection()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Подключение к БД успешно.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка подключения: " + ex.Message);
                return false;
            }
        }
    }

    public (int employeeId, string role)? LoginEmployee(string email, string password)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT EmployeeId, PasswordHash, Role FROM Employees WHERE LOWER(Email) = LOWER(@Email)";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email.ToLower());

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPasswordHash = reader.GetString("PasswordHash").Trim();
                            string role = reader.GetString("Role");
                            int employeeId = reader.GetInt32("EmployeeId");

                            if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                            {
                                return (employeeId, role);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Ошибка базы данных при попытке входа: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка при попытке входа: {ex.Message}");
                throw;
            }
        }
        return null;
    }

    public bool AddEmployee(string name, string email, string plainPassword, string role)
    {
        using (var checkConnection = new MySqlConnection(connectionString))
        {
            try
            {
                checkConnection.Open();
                string checkQuery = "SELECT COUNT(*) FROM Employees WHERE LOWER(Email) = LOWER(@Email)";
                using (var checkCmd = new MySqlCommand(checkQuery, checkConnection))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email.ToLower());
                    int existingUserCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (existingUserCount > 0)
                    {
                        MessageBox.Show("Работник с таким Email уже существует.", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error during employee email check: {ex.Message}");
                MessageBox.Show("Ошибка базы данных при проверке Email: " + ex.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during employee email check: {ex.Message}");
                MessageBox.Show("Общая ошибка при проверке Email: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                string query = "INSERT INTO Employees (Name, Email, PasswordHash, Role) VALUES (@Name, @Email, @PasswordHash, @Role)";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email.ToLower());
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@Role", role);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Ошибка базы данных при добавлении работника: " + ex.Message);
                MessageBox.Show("Ошибка базы данных при добавлении работника: " + ex.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Общая ошибка при добавлении работника: " + ex.Message);
                MessageBox.Show("Общая ошибка при добавлении работника: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    public DataTable GetRecyclingPoints()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT PointId, Name, Address, PhoneNumber FROM RecyclingPoints";

            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

    public bool AddRecyclingPoint(string name, string address, string phone)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO RecyclingPoints (Name, Address, PhoneNumber) VALUES (@Name, @Address, @Phone)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Phone", phone);
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка добавления пункта: " + ex.Message);
                    return false;
                }
            }
        }
    }

    public bool UpdateRecyclingPoint(int pointId, string name, string address, string phone)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "UPDATE RecyclingPoints SET Name = @Name, Address = @Address, PhoneNumber = @Phone WHERE PointId = @PointId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@PointId", pointId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Phone", phone);
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка обновления пункта: " + ex.Message);
                    return false;
                }
            }
        }
    }

    public bool DeleteRecyclingPoint(int pointId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM RecyclingPoints WHERE PointId = @PointId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@PointId", pointId);
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка удаления пункта: " + ex.Message);
                    return false;
                }
            }
        }
    }

    public DataTable GetAllRequests()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
            SELECT r.RequestId AS RequestId,
                   c.Model AS CarModel,
                   rp.Name AS RecyclingPoint,
                   e.Name AS EmployeeName,
                   r.Status,
                   r.SubmissionDate,
                   r.CompletionDate,
                   r.Cost,
                   r.Description,
                   r.WorkerComment,
                   r.AdminComment
            FROM Requests r
            JOIN Cars c ON r.CarId = c.CarId
            JOIN RecyclingPoints rp ON r.RecyclingPointId = rp.PointId
            LEFT JOIN Employees e ON r.EmployeeId = e.EmployeeId
            ORDER BY r.SubmissionDate DESC";

            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                Console.WriteLine("Столбцы в полученном DataTable:");
                foreach (DataColumn col in dt.Columns)
                {
                    Console.WriteLine($"- {col.ColumnName}");
                }

                if (!dt.Columns.Contains("RequestId"))
                {
                    Console.WriteLine("ОШИБКА: DataTable НЕ СОДЕРЖИТ столбец 'RequestId'!");
                    throw new InvalidOperationException("DataTable не содержит ожидаемый столбец 'RequestId'. Проверьте SQL-запрос и схему БД.");
                }

                return dt;
            }
        }
    }

    public bool MarkRequestAsAwaitingApproval(int requestId)
    {
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = "UPDATE Requests SET Status = 'Ожидает подтверждения' WHERE RequestId = @id";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", requestId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool UpdateRequestStatus(int requestId, string newStatus, decimal? cost = null)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = newStatus == "Завершено"
                ? @"UPDATE Requests 
                        SET Status = @Status, CompletionDate = NOW(), Cost = @Cost 
                        WHERE RequestId = @RequestId"
                : @"UPDATE Requests 
                        SET Status = @Status 
                        WHERE RequestId = @RequestId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                if (newStatus == "Завершено")
                {
                    cmd.Parameters.AddWithValue("@Cost", cost ?? 0);
                }

                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка обновления статуса: " + ex.Message);
                    return false;
                }
            }
        }
    }

    public DataTable GetWorkers()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT EmployeeId, Name FROM Employees WHERE Role = 'worker'";

            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

    public bool AssignRequestToWorker(int requestId, int workerId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "UPDATE Requests SET EmployeeId = @WorkerId WHERE RequestId = @RequestId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@WorkerId", workerId);
                cmd.Parameters.AddWithValue("@RequestId", requestId);

                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка присваивания заявки: " + ex.Message);
                    return false;
                }
            }
        }
    }

    public DataTable GetRequestsByWorkerId(int workerId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                    SELECT r.RequestId,
                           c.Model AS CarModel,
                           rp.Name AS RecyclingPoint,
                           e.Name AS EmployeeName,
                           r.Status,
                           r.SubmissionDate,
                           r.CompletionDate,
                           r.Cost,
                           r.Description,
                           r.AdminComment,
                           r.WorkerComment
                    FROM Requests r
                    JOIN Cars c ON r.CarId = c.CarId
                    JOIN RecyclingPoints rp ON r.RecyclingPointId = rp.PointId
                    LEFT JOIN Employees e ON r.EmployeeId = e.EmployeeId
                    WHERE r.EmployeeId = @workerId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@workerId", workerId);

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public DataTable GetWorkerRequests(int workerId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                SELECT r.RequestId,
                     c.Model AS CarModel,
                     rp.Name AS RecyclingPoint,
                     e.Name AS EmployeeName,
                     r.Status,
                     r.SubmissionDate,
                     r.CompletionDate,
                     r.Cost,
                     r.Description,
                     r.AdminComment,
                     r.WorkerComment
                FROM Requests r
                JOIN Cars c ON r.CarId = c.CarId
                JOIN RecyclingPoints rp ON r.RecyclingPointId = rp.PointId
                LEFT JOIN Employees e ON r.EmployeeId = e.EmployeeId
                WHERE r.EmployeeId = @workerId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@WorkerId", workerId);

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public bool CompleteRequest(int requestId, string status)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"UPDATE Requests 
                             SET Status = @Status, 
                                 CompletionDate = NOW(), 
                                 AdminConfirmed = TRUE,
                                 AdminComment = NULL
                             WHERE RequestId = @RequestId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool RejectRequest(int requestId, string comment)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"UPDATE Requests 
                             SET Status = 'Требует доработки', 
                                 AdminComment = @Comment,
                                 AdminConfirmed = FALSE
                             WHERE RequestId = @RequestId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Comment", comment);
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public DataTable GetAllClients()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = @"
                    SELECT
                        ClientId,
                        FirstName,
                        LastName,
                        Email,
                        PhoneNumber
                    FROM Clients
                    ORDER BY FirstName, LastName";

                using (var adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Ошибка базы данных при получении клиентов: {ex.Message}");
                MessageBox.Show($"Ошибка базы данных при получении клиентов: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка при получении клиентов: {ex.Message}");
                MessageBox.Show($"Общая ошибка при получении клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }

    public bool AddOrUpdateWorkerComment(int requestId, string workerComment)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "UPDATE Requests SET WorkerComment = @Comment WHERE RequestId = @RequestId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Comment", string.IsNullOrWhiteSpace(workerComment) ? (object)DBNull.Value : workerComment);
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка добавления/обновления комментария работника: " + ex.Message);
                    return false;
                }
            }
        }
    }
}