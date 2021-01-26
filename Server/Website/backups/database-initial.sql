-- phpMyAdmin SQL Dump
-- version 4.9.5
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Dec 23, 2020 at 02:08 PM
-- Server version: 5.7.31-cll-lve
-- PHP Version: 7.3.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";

--
-- Database: `mpnmjeca_feedback`
--

-- --------------------------------------------------------

--
-- Table structure for table `account`
--

CREATE TABLE `account` (
  `deptname` varchar(5) NOT NULL,
  `deptcode` int(3) NOT NULL,
  `password` varchar(10) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `account`
--

INSERT INTO `account` (`deptname`, `deptcode`, `password`) VALUES
('CIVIL', 103, 'civil'),
('CSE', 104, 'cse'),
('EEE', 105, 'eee'),
('ECE', 106, 'ece'),
('MECH', 114, 'mech'),
('IT', 205, 'it'),
('MCA', 621, 'mca'),
('MBA', 631, 'mba'),
('ENG', 0, 'eng'),
('CHEM', 0, 'chem'),
('MATH', 0, 'math'),
('PHY', 0, 'phy');

-- --------------------------------------------------------

--
-- Table structure for table `advisor`
--

CREATE TABLE `advisor` (
  `classcode` varchar(9) NOT NULL,
  `password` varchar(10) NOT NULL,
  `dept` varchar(5) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `college`
--

CREATE TABLE `college` (
  `date` varchar(10) NOT NULL,
  `regno` varchar(12) NOT NULL,
  `semester` varchar(5) NOT NULL,
  `branch` varchar(5) NOT NULL,
  `r1` int(2) NOT NULL,
  `r2` varchar(6) NOT NULL,
  `r31` int(2) NOT NULL,
  `r32` int(2) DEFAULT NULL,
  `r33` int(2) DEFAULT NULL,
  `r34` int(2) DEFAULT NULL,
  `r35` int(2) DEFAULT NULL,
  `r4` int(2) NOT NULL,
  `r5` int(2) NOT NULL,
  `r6` varchar(6) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `feedback`
--

CREATE TABLE `feedback` (
  `date` varchar(10) NOT NULL,
  `regno` varchar(12) DEFAULT NULL,
  `semester` varchar(4) NOT NULL,
  `branch` varchar(6) NOT NULL,
  `staffid` varchar(8) NOT NULL,
  `staffname` varchar(28) DEFAULT NULL,
  `staffdept` varchar(6) NOT NULL,
  `subject` varchar(6) NOT NULL,
  `code` varchar(6) NOT NULL,
  `s1` int(2) NOT NULL,
  `s2` int(2) NOT NULL,
  `s3` int(2) NOT NULL,
  `s4` int(2) NOT NULL,
  `s5` int(2) NOT NULL,
  `s6` int(2) NOT NULL,
  `s7` int(2) NOT NULL,
  `s8` int(2) NOT NULL,
  `s9` int(2) NOT NULL,
  `s10` int(2) NOT NULL,
  `total` int(4) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `staff`
--

CREATE TABLE `staff` (
  `id` varchar(8) NOT NULL,
  `pre` varchar(4) NOT NULL,
  `name` varchar(20) NOT NULL,
  `des` varchar(4) NOT NULL,
  `dept` varchar(5) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `student`
--

CREATE TABLE `student` (
  `regno` varchar(12) NOT NULL,
  `dob` varchar(10) NOT NULL,
  `name` varchar(20) NOT NULL,
  `status` int(1) NOT NULL DEFAULT '0',
  `cf` int(1) NOT NULL DEFAULT '0',
  `s1` int(1) NOT NULL DEFAULT '0',
  `s2` int(1) NOT NULL DEFAULT '0',
  `s3` int(1) NOT NULL DEFAULT '0',
  `s4` int(1) NOT NULL DEFAULT '0',
  `s5` int(1) NOT NULL DEFAULT '0',
  `s6` int(1) NOT NULL DEFAULT '0',
  `s7` int(1) NOT NULL DEFAULT '0',
  `s8` int(1) NOT NULL DEFAULT '0',
  `s9` int(1) NOT NULL DEFAULT '0',
  `s10` int(1) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `subject`
--

CREATE TABLE `subject` (
  `classcode` varchar(9) NOT NULL,
  `semester` varchar(5) NOT NULL,
  `branch` varchar(5) NOT NULL,
  `subject` varchar(6) NOT NULL,
  `code` varchar(6) NOT NULL,
  `staffid` varchar(8) NOT NULL,
  `staffname` varchar(28) NOT NULL,
  `staffdept` varchar(5) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `account`
--
ALTER TABLE `account`
  ADD PRIMARY KEY (`deptname`);

--
-- Indexes for table `advisor`
--
ALTER TABLE `advisor`
  ADD PRIMARY KEY (`classcode`);

--
-- Indexes for table `staff`
--
ALTER TABLE `staff`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `student`
--
ALTER TABLE `student`
  ADD PRIMARY KEY (`regno`);
COMMIT;
