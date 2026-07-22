using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CarRecyclingWeb.Models;
using CarRecyclingWeb.Extensions;

namespace CarRecyclingWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RecyclingPoint> RecyclingPoints { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Request)
                .WithMany(r => r.Feedbacks)
                .HasForeignKey(f => f.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Client)
                .WithMany()
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.CarId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.ClientId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.RecyclingPoint)
                .WithMany(rp => rp.Requests)
                .HasForeignKey(r => r.RecyclingPointId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Employee)
                .WithMany(e => e.Requests)
                .HasForeignKey(r => r.EmployeeId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Cars)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Car>()
                .HasIndex(c => c.VIN)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Role)
                .HasConversion<string>();

            var requestStatusConverter = new ValueConverter<RequestStatus, string>(
                v => v.GetEnumMemberValue(),
                v => v.ToEnum<RequestStatus>()
            );
            modelBuilder.Entity<Request>()
                .Property(r => r.Status)
                .HasConversion(requestStatusConverter);

            modelBuilder.Entity<Car>()
                .Property(c => c.VehicleType)
                .HasConversion<string>();
        }

        public void SeedData()
        {
            if (Employees.Any(e => e.Role == EmployeeRole.admin) && Clients.Any())
            {
                Console.WriteLine("База данных уже содержит начальные данные, пропускаем засеивание.");
                return;
            }

            Console.WriteLine("Засеивание базы данных начальными данными...");

            if (!Employees.Any(e => e.Email == "admin@example.com" && e.Role == EmployeeRole.admin))
            {
                var adminPassword = PasswordHasher.HashPassword("Admin123!");
                var admin = new Employee
                {
                    Name = "Администратор",
                    Email = "admin@example.com",
                    PasswordHash = adminPassword,
                    Role = EmployeeRole.admin
                };
                Employees.Add(admin);
                SaveChanges();
                Console.WriteLine($"Администратор '{admin.Email}' создан.");
            }
            else
            {
                Console.WriteLine("Администратор уже существует.");
            }
            var adminUser = Employees.FirstOrDefault(e => e.Email == "admin@example.com");

            if (!Employees.Any(e => e.Email == "worker@example.com" && e.Role == EmployeeRole.worker))
            {
                var workerPassword = PasswordHasher.HashPassword("Worker123!");
                var worker = new Employee
                {
                    Name = "Работник",
                    Email = "worker@example.com",
                    PasswordHash = workerPassword,
                    Role = EmployeeRole.worker
                };
                Employees.Add(worker);
                SaveChanges();
                Console.WriteLine($"Работник '{worker.Email}' создан.");
            }
            else
            {
                Console.WriteLine("Работник уже существует.");
            }
            var workerUser = Employees.FirstOrDefault(e => e.Email == "worker@example.com");

            if (!Clients.Any(c => c.Email == "ivan@example.com"))
            {
                var clientPassword = PasswordHasher.HashPassword("Client123!");
                var client = new Client
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Email = "ivan@example.com",
                    PasswordHash = clientPassword,
                    PhoneNumber = "+79123456789"
                };
                Clients.Add(client);
                SaveChanges();
                Console.WriteLine($"Клиент '{client.Email}' создан.");
            }
            else
            {
                Console.WriteLine("Клиент уже существует.");
            }
            var clientUser = Clients.FirstOrDefault(c => c.Email == "ivan@example.com");

            if (!RecyclingPoints.Any(rp => rp.Name == "Центр утилизации №1"))
            {
                var recyclingPoint = new RecyclingPoint
                {
                    Name = "Центр утилизации №1",
                    Address = "г. Москва, ул. Примерная, 10",
                    PhoneNumber = "+74951234567",
                    MapUrl = "https://maps.google.com/?q=Москва+Примерная+10",
                    OpeningHours = "Пн-Пт 9:00-18:00",
                    Description = "Основной пункт приема автомобилей на утилизацию.",
                    ImageUrl = "https://example.com/recycling_point_1.jpg"
                };
                RecyclingPoints.Add(recyclingPoint);
                SaveChanges();
                Console.WriteLine($"Пункт утилизации '{recyclingPoint.Name}' создан.");
            }
            else
            {
                Console.WriteLine("Пункт утилизации уже существует.");
            }
            var recyclingPointEntity = RecyclingPoints.FirstOrDefault(rp => rp.Name == "Центр утилизации №1");

            if (clientUser == null || recyclingPointEntity == null || workerUser == null)
            {
                Console.WriteLine("Не удалось найти необходимые сущности для создания автомобиля или заявки. Пропуск создания.");
                return;
            }

            if (!Cars.Any(c => c.VIN == "ABC123DEF456GHI78" && c.ClientId == clientUser.ClientId))
            {
                var car = new Car
                {
                    Brand = "Лада",
                    Model = "Приора",
                    Year = 2008,
                    VIN = "ABC123DEF456GHI78",
                    LicensePlate = "А123БВ 777",
                    WeightKg = 1200.50m,
                    VehicleType = VehicleType.Легковой,
                    ClientId = clientUser.ClientId
                };
                Cars.Add(car);
                SaveChanges();
                Console.WriteLine($"Автомобиль '{car.Brand} {car.Model}' для клиента '{clientUser.Email}' создан.");
            }
            else
            {
                Console.WriteLine($"Автомобиль с VIN 'ABC123DEF456GHI78' для клиента '{clientUser.Email}' уже существует.");
            }
            var carEntity = Cars.FirstOrDefault(c => c.VIN == "ABC123DEF456GHI78");

            if (carEntity != null && !Requests.Any(r => r.CarId == carEntity.CarId && r.ClientId == clientUser.ClientId))
            {
                var request = new Request
                {
                    CarId = carEntity.CarId,
                    ClientId = clientUser.ClientId,
                    RecyclingPointId = recyclingPointEntity.PointId,
                    EmployeeId = workerUser.EmployeeId,
                    Status = RequestStatus.Accepted,
                    SubmissionDate = DateTime.Now,
                    PreferredDisposalDate = DateTime.Now.AddDays(7),
                    Condition = "Плохое",
                    Description = "Автомобиль после ДТП, не на ходу.",
                    AdminConfirmed = false,
                    WorkerComment = "Ожидает первичной проверки.",
                    AdminComment = null
                };
                Requests.Add(request);
                SaveChanges();
                Console.WriteLine($"Заявка для автомобиля '{carEntity.VIN}' создана.");
            }
            else
            {
                Console.WriteLine($"Заявка для автомобиля с VIN '{carEntity?.VIN}' уже существует или автомобиль не найден.");
            }

            Console.WriteLine("Засеивание данных завершено.");
        }
    }
}