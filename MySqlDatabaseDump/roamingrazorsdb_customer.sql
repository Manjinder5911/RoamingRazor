-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: roamingrazorsdb
-- ------------------------------------------------------
-- Server version	8.0.36

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
-- Table structure for table `customer`
--

DROP TABLE IF EXISTS `customer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customer` (
  `Customer_ID` int NOT NULL AUTO_INCREMENT,
  `Customer_Name` varchar(45) NOT NULL,
  `Customer_Pwd` varchar(45) NOT NULL,
  `Full_Name` varchar(45) NOT NULL,
  `Address` varchar(250) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Phone_Number` char(10) NOT NULL,
  `Status_ID` int NOT NULL,
  PRIMARY KEY (`Customer_ID`),
  UNIQUE KEY `user_name_UNIQUE` (`Customer_Name`),
  KEY `customer_statusFK_idx` (`Status_ID`),
  CONSTRAINT `customer_statusFK` FOREIGN KEY (`Status_ID`) REFERENCES `status` (`Status_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customer`
--

LOCK TABLES `customer` WRITE;
/*!40000 ALTER TABLE `customer` DISABLE KEYS */;
INSERT INTO `customer` VALUES (1,'test@email.com','test','','','','',1),(2,'tst','tst','Test1','14708 111ave, surrey, BC, V3R2C4, Canada','test@email.com','1234567890',1),(3,'tst1','tst','Test1','14708 111ave, surrey, BC, V3R2C4, Canada','test@email.com','1234567890',1),(4,'manji','test','Manjinder','13915 100Ave,Surrey,British Columbia,Canada,V3T 1J4','812572@columbiacollege.ca','6046522074',1),(5,'manjinder','test','Manjinder Singh','14708 111 Ave,Surrey,British Columbia,Canada,V3R 2C4','manjindersekhon2003@gmail.com','6046522074',1),(6,'lokesh','lokesh','lokesh','13915 100Ave,Surrey,British Columbia,Canada,V3T 1J4','lokesh@gmail.com','1234567890',1),(7,'loki','loki','lokesh','13915 100Ave,Surrey,British Columbia,Canada,V3T 1J4','lokesh124@gmail.com','1234567891',1),(8,'lokeshKakkar','lokeshKakkar','Lokesh','13915 100Ave,Surrey,British Columbia,Canada,V3T 1J4','lkakkar2002@gmail.com','6046522074',1);
/*!40000 ALTER TABLE `customer` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-04-04 12:05:47
