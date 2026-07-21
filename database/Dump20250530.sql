CREATE DATABASE  IF NOT EXISTS `carrecyclingemp` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `carrecyclingemp`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: carrecyclingemp
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20250530003252_FinalMigrationAttempt','7.0.2');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cars`
--

DROP TABLE IF EXISTS `cars`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cars` (
  `CarId` int NOT NULL AUTO_INCREMENT,
  `Brand` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Model` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Year` int NOT NULL,
  `VIN` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LicensePlate` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `WeightKg` decimal(10,2) DEFAULT NULL,
  `VehicleType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientId` int NOT NULL,
  PRIMARY KEY (`CarId`),
  UNIQUE KEY `IX_Cars_VIN` (`VIN`),
  KEY `IX_Cars_ClientId` (`ClientId`),
  CONSTRAINT `FK_Cars_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`ClientId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cars`
--

LOCK TABLES `cars` WRITE;
/*!40000 ALTER TABLE `cars` DISABLE KEYS */;
INSERT INTO `cars` VALUES (1,'Лада','Приора',2008,'ABC123DEF456GHI78','А123БВ 777',1200.50,'Легковой',1),(2,'Fiat ','Albea',2020,'12345678910121234','А123БВ 778',2000.00,'Легковой',2);
/*!40000 ALTER TABLE `cars` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clients` (
  `ClientId` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PhoneNumber` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ClientId`),
  UNIQUE KEY `IX_Clients_Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (1,'Иван','Иванов','ivan@example.com','$2a$11$v64PAitpr/r1RrCibZYnO./qClJJQGeaUrwP/um3Eg0FpJNBtrzpy','+79123456789'),(2,'Айдар','Мифтахов','aidarro14@gmail.com','$2a$11$WVtX7sPBcwHpVI2fil/p2ua4qYT6jo9DoEEkNYKzwJHCKcDRzXuRS','89869027282');
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employees` (
  `EmployeeId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`EmployeeId`),
  UNIQUE KEY `IX_Employees_Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (1,'Администратор','admin@example.com','$2a$11$2qdmAH9s.aLxdubzpyCTDukyLg8AiHcQV/Wn59lhh7g6NQu4OnJqW','admin'),(2,'Работник','worker@example.com','$2a$11$lRhdy3gIcXYDycXW6akZbenGlIbolI5G/OetXJYeWtY9tCraAlbGq','worker'),(3,'Albert','valbern@mail.com','$2a$11$TBEeWz.e5311BUAWKeJnnerDxghAUo9yKd9Pu6PudP5klAiYyx60S','worker');
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `feedbacks`
--

DROP TABLE IF EXISTS `feedbacks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `feedbacks` (
  `FeedbackId` int NOT NULL AUTO_INCREMENT,
  `RequestId` int NOT NULL,
  `ClientId` int NOT NULL,
  `Rating` int NOT NULL,
  `Comment` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SubmissionDate` datetime(6) NOT NULL,
  PRIMARY KEY (`FeedbackId`),
  KEY `IX_Feedbacks_ClientId` (`ClientId`),
  KEY `IX_Feedbacks_RequestId` (`RequestId`),
  CONSTRAINT `FK_Feedbacks_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`ClientId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Feedbacks_Requests_RequestId` FOREIGN KEY (`RequestId`) REFERENCES `requests` (`RequestId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `feedbacks`
--

LOCK TABLES `feedbacks` WRITE;
/*!40000 ALTER TABLE `feedbacks` DISABLE KEYS */;
INSERT INTO `feedbacks` VALUES (1,2,1,5,'Все сделали отлично','2025-05-30 04:28:09.085535');
/*!40000 ALTER TABLE `feedbacks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recyclingpoints`
--

DROP TABLE IF EXISTS `recyclingpoints`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recyclingpoints` (
  `PointId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PhoneNumber` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MapUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `OpeningHours` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ImageUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`PointId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recyclingpoints`
--

LOCK TABLES `recyclingpoints` WRITE;
/*!40000 ALTER TABLE `recyclingpoints` DISABLE KEYS */;
INSERT INTO `recyclingpoints` VALUES (2,'ЭкоТех Казань','г. Казань, ул. Кремлёвская, 15','+7 843 555-1234','https://www.google.ru/maps/place/%D0%9A%D1%80%D0%B5%D0%BC%D0%BB%D0%B5%D0%B2%D1%81%D0%BA%D0%B0%D1%8F+%D0%BD%D0%B0%D0%B1.,+15,+%D0%9A%D0%B0%D0%B7%D0%B0%D0%BD%D1%8C,+%D0%A0%D0%B5%D1%81%D0%BF.+%D0%A2%D0%B0%D1%82%D0%B0%D1%80%D1%81%D1%82%D0%B0%D0%BD,+%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F,+420111/@55.8020344,49.1006707,17z/data=!3m1!4b1!4m9!1m2!2m1!1z0YPQuy4g0JrRgNC10LzQu9C10LLRgdC60LDRjyDQvdCw0LEuLCAxNSwg0JrQsNC30LDQvdGMLCDQoNC-0YHRgdC40Y8!3m5!1s0x415ead397e9c3bab:0x479a0cc5da7fab32!8m2!3d55.8020345!4d49.1055416!15sCkTRg9C7LiDQmtGA0LXQvNC70LXQstGB0LrQsNGPINC90LDQsS4sIDE1LCDQmtCw0LfQsNC90YwsINCg0L7RgdGB0LjRj5IBEGdlb2NvZGVkX2FkZHJlc3PgAQA?entry=ttu&g_ep=EgoyMDI1MDUyNy4wIKXMDSoASAFQAw%3D%3D','Часы работы: Пн-Пт: 09:00-18:00, Сб: 10:00-14:00','Центральный пункт утилизации в черте города. Принимаем все виды автотранспорта и запчастей.','/images/1.png'),(3,'ВторРесурс Казань','г. Казань, ул. Чистопольская, 21','+7 843 777-4321','https://www.google.ru/maps/place/%D1%83%D0%BB.+%D0%A7%D0%B8%D1%81%D1%82%D0%BE%D0%BF%D0%BE%D0%BB%D1%8C%D1%81%D0%BA%D0%B0%D1%8F,+21,+%D0%9A%D0%B0%D0%B7%D0%B0%D0%BD%D1%8C,+%D0%A0%D0%B5%D1%81%D0%BF.+%D0%A2%D0%B0%D1%82%D0%B0%D1%80%D1%81%D1%82%D0%B0%D0%BD,+%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F,+420124/@55.8194328,49.1130671,17z/data=!3m1!4b1!4m5!3m4!1s0x415ead527a5403af:0x93885c8143dcbdbd!8m2!3d55.8194328!4d49.115642?entry=ttu&g_ep=EgoyMDI1MDUyNy4wIKXMDSoASAFQAw%3D%3Dn','Часы работы: Ежедневно: 08:30-17:00 (перерыв 13:00-14:00)','Специализированный пункт по утилизации грузовых автомобилей и спецтехники. Требуется предварительная запись.','/images/2.png');
/*!40000 ALTER TABLE `recyclingpoints` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requests`
--

DROP TABLE IF EXISTS `requests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `requests` (
  `RequestId` int NOT NULL AUTO_INCREMENT,
  `CarId` int NOT NULL,
  `ClientId` int NOT NULL,
  `RecyclingPointId` int NOT NULL,
  `EmployeeId` int DEFAULT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SubmissionDate` datetime(6) NOT NULL,
  `PreferredDisposalDate` datetime(6) NOT NULL,
  `Condition` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CompletionDate` datetime(6) DEFAULT NULL,
  `Cost` decimal(10,2) DEFAULT NULL,
  `AdminConfirmed` tinyint(1) NOT NULL,
  `WorkerComment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AdminComment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`RequestId`),
  KEY `IX_Requests_CarId` (`CarId`),
  KEY `IX_Requests_ClientId` (`ClientId`),
  KEY `IX_Requests_EmployeeId` (`EmployeeId`),
  KEY `IX_Requests_RecyclingPointId` (`RecyclingPointId`),
  CONSTRAINT `FK_Requests_Cars_CarId` FOREIGN KEY (`CarId`) REFERENCES `cars` (`CarId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Requests_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`ClientId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Requests_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `employees` (`EmployeeId`),
  CONSTRAINT `FK_Requests_RecyclingPoints_RecyclingPointId` FOREIGN KEY (`RecyclingPointId`) REFERENCES `recyclingpoints` (`PointId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requests`
--

LOCK TABLES `requests` WRITE;
/*!40000 ALTER TABLE `requests` DISABLE KEYS */;
INSERT INTO `requests` VALUES (2,1,1,2,2,'Завершена','2025-05-20 04:09:47.000000','2025-05-25 04:09:47.000000','Отличное','Машина разобрана на запчасти.','2025-05-30 04:14:38.000000',1500.00,1,'Утилизация завершена, акт подписан.',NULL),(3,2,2,2,NULL,'Принята','2025-05-30 04:42:15.752789','2025-06-06 00:00:00.000000','Рабочее','aaa',NULL,5000.00,0,NULL,NULL);
/*!40000 ALTER TABLE `requests` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-30 13:49:42
