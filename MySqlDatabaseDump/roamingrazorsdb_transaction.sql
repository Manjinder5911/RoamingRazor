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
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction` (
  `Transaction_ID` int NOT NULL AUTO_INCREMENT,
  `Order_ID` int NOT NULL,
  `Payment_Type_ID` int NOT NULL,
  `Transaction_Successful` tinyint NOT NULL,
  `Stylist_Paid` tinyint NOT NULL,
  `Transaction_Amount` float(5,2) NOT NULL,
  `Stylist_Amount` float(5,2) NOT NULL,
  `Date_Time` datetime NOT NULL,
  `Customer_Reference_ID` varchar(300) NOT NULL,
  `Paypal_Transaction_ID` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Transaction_ID`),
  KEY `transaction_paymentFK_idx` (`Payment_Type_ID`),
  KEY `transaction_orderFK_idx` (`Order_ID`),
  CONSTRAINT `transaction_orderFK` FOREIGN KEY (`Order_ID`) REFERENCES `order` (`Order_ID`),
  CONSTRAINT `transaction_paymentFK` FOREIGN KEY (`Payment_Type_ID`) REFERENCES `payment` (`Payment_Type_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction`
--

LOCK TABLES `transaction` WRITE;
/*!40000 ALTER TABLE `transaction` DISABLE KEYS */;
INSERT INTO `transaction` VALUES (7,25,1,1,0,56.00,50.40,'2024-03-24 10:20:19','UeAy1757Y1','PAYID-MYAGBEY72K1481436301590D'),(8,26,1,1,0,56.00,50.40,'2024-03-24 10:35:02','3zmGksohFS','PAYID-MYAGIEI6V208220TX7466931'),(9,27,1,1,0,33.60,30.24,'2024-03-25 11:35:33','WCZgizNAjH','PAYID-MYA4HNA0E552561F46350128'),(10,28,1,1,0,56.00,50.40,'2024-03-29 16:43:19','KIkou4jcHw','PAYID-MYDVDVQ738338572C0097511'),(11,29,1,1,0,67.20,60.48,'2024-04-02 15:21:19','kehGQzlIrG','PAYID-MYGIJJQ2N443258DT987123Y'),(12,31,1,1,0,67.20,60.48,'2024-04-03 13:42:33','Q8D08jx1ld','PAYID-MYG352I14Y61954WN1797940'),(13,32,1,1,0,67.20,60.48,'2024-04-04 10:44:15','oFSW6Rjhtt','PAYID-MYHONLA46U264191C988924J');
/*!40000 ALTER TABLE `transaction` ENABLE KEYS */;
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
