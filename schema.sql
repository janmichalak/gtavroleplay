-- phpMyAdmin SQL Dump
-- version 4.7.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 12 Lis 2017, 23:22
-- Wersja serwera: 10.1.26-MariaDB
-- Wersja PHP: 7.1.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Baza danych: `lsrpv`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `characters`
--

CREATE TABLE `characters` (
  `cid` int(11) NOT NULL,
  `owner` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `hours` int(11) NOT NULL DEFAULT '0',
  `minutes` int(11) NOT NULL DEFAULT '0',
  `health` float NOT NULL DEFAULT '100',
  `gender` int(11) NOT NULL DEFAULT '1',
  `skin` int(11) NOT NULL DEFAULT '225514697',
  `money` int(11) NOT NULL DEFAULT '0',
  `bankmoney` int(11) NOT NULL DEFAULT '0',
  `dob` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `dimension` int(11) NOT NULL DEFAULT '0',
  `posx` float NOT NULL DEFAULT '0',
  `posy` float NOT NULL DEFAULT '0',
  `posz` float NOT NULL DEFAULT '0',
  `bw` int(11) NOT NULL DEFAULT '0',
  `online` int(11) NOT NULL DEFAULT '0',
  `crash` int(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `doors`
--

CREATE TABLE `doors` (
  `uid` int(11) NOT NULL,
  `owner` int(11) NOT NULL DEFAULT '0',
  `ownertype` int(11) NOT NULL DEFAULT '0',
  `name` varchar(255) NOT NULL DEFAULT 'Drzwi',
  `enterx` float NOT NULL,
  `entery` float NOT NULL,
  `enterz` float NOT NULL,
  `exitx` float NOT NULL DEFAULT '0',
  `exity` float NOT NULL DEFAULT '0',
  `exitz` float NOT NULL DEFAULT '0',
  `entervw` int(11) NOT NULL,
  `exitvw` int(11) NOT NULL DEFAULT '0',
  `enterangle` float NOT NULL DEFAULT '0',
  `exitangle` float NOT NULL DEFAULT '0',
  `markertype` int(11) NOT NULL DEFAULT '0',
  `colr` int(4) NOT NULL DEFAULT '255',
  `colg` int(4) NOT NULL DEFAULT '255',
  `colb` int(4) NOT NULL DEFAULT '255',
  `alpha` int(4) NOT NULL DEFAULT '255'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `items`
--

CREATE TABLE `items` (
  `uid` int(11) NOT NULL,
  `type` int(11) NOT NULL DEFAULT '0',
  `owner` int(11) NOT NULL DEFAULT '0',
  `place` int(11) NOT NULL DEFAULT '0',
  `name` varchar(64) NOT NULL DEFAULT 'Przedmiot',
  `value1` int(11) NOT NULL DEFAULT '0',
  `value2` int(11) NOT NULL DEFAULT '0',
  `value3` int(11) NOT NULL DEFAULT '0',
  `str1` varchar(255) NOT NULL DEFAULT '',
  `str2` varchar(255) NOT NULL DEFAULT '',
  `str3` varchar(255) NOT NULL DEFAULT '',
  `posx` float NOT NULL DEFAULT '0',
  `posy` float NOT NULL DEFAULT '0',
  `posz` float NOT NULL DEFAULT '0',
  `dimension` int(11) NOT NULL DEFAULT '0',
  `use` int(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `hash` varchar(255) NOT NULL,
  `admin` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `vehicles`
--

CREATE TABLE `vehicles` (
  `veh_id` int(11) NOT NULL,
  `veh_model` int(11) NOT NULL,
  `veh_ownertype` int(11) NOT NULL DEFAULT '0',
  `veh_owner` int(11) NOT NULL DEFAULT '0',
  `veh_hp` float NOT NULL DEFAULT '1000',
  `veh_posx` float NOT NULL,
  `veh_posy` float NOT NULL,
  `veh_posz` float NOT NULL,
  `veh_rotx` float NOT NULL,
  `veh_roty` float NOT NULL,
  `veh_rotz` float NOT NULL,
  `veh_col1` int(11) NOT NULL DEFAULT '0',
  `veh_col2` int(11) NOT NULL DEFAULT '0',
  `veh_vw` int(11) NOT NULL DEFAULT '1',
  `veh_fuel` float NOT NULL DEFAULT '50',
  `veh_oil` float NOT NULL DEFAULT '2.5',
  `veh_tyres` varchar(12) NOT NULL DEFAULT '0,0,0,0,0,0',
  `veh_plate` varchar(16) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indeksy dla zrzutów tabel
--

--
-- Indexes for table `characters`
--
ALTER TABLE `characters`
  ADD PRIMARY KEY (`cid`);

--
-- Indexes for table `doors`
--
ALTER TABLE `doors`
  ADD PRIMARY KEY (`uid`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`uid`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `vehicles`
--
ALTER TABLE `vehicles`
  ADD PRIMARY KEY (`veh_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT dla tabeli `characters`
--
ALTER TABLE `characters`
  MODIFY `cid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT dla tabeli `doors`
--
ALTER TABLE `doors`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
--
-- AUTO_INCREMENT dla tabeli `items`
--
ALTER TABLE `items`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;
--
-- AUTO_INCREMENT dla tabeli `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT dla tabeli `vehicles`
--
ALTER TABLE `vehicles`
  MODIFY `veh_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
