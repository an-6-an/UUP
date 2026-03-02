-- Создание базы данных
CREATE DATABASE IF NOT EXISTS autoservice_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE autoservice_db;

-- Таблица пользователей
CREATE TABLE IF NOT EXISTS Users (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    FIO VARCHAR(200) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Login VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Type VARCHAR(50) NOT NULL
);

-- Таблица заявок
CREATE TABLE IF NOT EXISTS Requests (
    RequestID INT PRIMARY KEY AUTO_INCREMENT,
    StartDate DATETIME NOT NULL,
    CarType VARCHAR(50) NOT NULL,
    CarModel VARCHAR(200) NOT NULL,
    ProblemDescription TEXT NOT NULL,
    RequestStatus VARCHAR(50) NOT NULL DEFAULT 'Новая заявка',
    CompletionDate DATETIME NULL,
    RepairParts TEXT NULL,
    MasterID INT NULL,
    ClientID INT NOT NULL,
    FOREIGN KEY (MasterID) REFERENCES Users(UserID) ON DELETE SET NULL,
    FOREIGN KEY (ClientID) REFERENCES Users(UserID) ON DELETE RESTRICT
);

-- Таблица комментариев
CREATE TABLE IF NOT EXISTS Comments (
    CommentID INT PRIMARY KEY AUTO_INCREMENT,
    Message TEXT NOT NULL,
    MasterID INT NOT NULL,
    RequestID INT NOT NULL,
    FOREIGN KEY (MasterID) REFERENCES Users(UserID) ON DELETE RESTRICT,
    FOREIGN KEY (RequestID) REFERENCES Requests(RequestID) ON DELETE CASCADE
);

-- Тестовые данные — пользователи
INSERT IGNORE INTO Users (UserID, FIO, Phone, Login, Password, Type) VALUES
(1, 'Белов Александр Давидович', '89210563128', 'login1', 'pass1', 'Менеджер'),
(2, 'Харитонова Мария Павловна', '89535078985', 'login2', 'pass2', 'Автомеханик'),
(3, 'Марков Давид Иванович', '89210673849', 'login3', 'pass3', 'Автомеханик'),
(4, 'Громова Анна Семёновна', '89990563748', 'login4', 'pass4', 'Оператор'),
(5, 'Карташова Мария Данииловна', '89994563847', 'login5', 'pass5', 'Оператор'),
(6, 'Касаткин Егор Львович', '89219567849', 'login11', 'pass11', 'Заказчик'),
(7, 'Ильина Тамара Даниловна', '89219567841', 'login12', 'pass12', 'Заказчик'),
(8, 'Елисеева Юлиана Алексеевна', '89219567842', 'login13', 'pass13', 'Заказчик'),
(9, 'Никифорова Алиса Тимофеевна', '89219567843', 'login14', 'pass14', 'Заказчик'),
(10, 'Васильев Али Евгеньевич', '89219567844', 'login15', 'pass15', 'Автомеханик');

-- Тестовые данные — заявки
INSERT IGNORE INTO Requests (RequestID, StartDate, CarType, CarModel, ProblemDescription, RequestStatus, CompletionDate, RepairParts, MasterID, ClientID) VALUES
(1, '2023-06-06', 'Легковая', 'Hyundai Avante (CN7)', 'Отказали тормоза.', 'В процессе ремонта', NULL, NULL, 2, 7),
(2, '2023-05-05', 'Легковая', 'Nissan 180SX', 'Отказали тормоза.', 'В процессе ремонта', NULL, NULL, 3, 8),
(3, '2022-07-07', 'Легковая', 'Toyota 2000GT', 'В салоне пахнет бензином.', 'Готова к выдаче', '2023-01-01', NULL, 3, 9),
(4, '2023-08-02', 'Грузовая', 'Citroen Berlingo (B9)', 'Руль плохо крутится.', 'Новая заявка', NULL, NULL, NULL, 8),
(5, '2023-08-02', 'Грузовая', 'УАЗ 2360', 'Руль плохо крутится.', 'Новая заявка', NULL, NULL, NULL, 9);

-- Тестовые данные — комментарии
INSERT IGNORE INTO Comments (CommentID, Message, MasterID, RequestID) VALUES
(1, 'Очень странно.', 2, 1),
(2, 'Будем разбираться!', 3, 2),
(3, 'Будем разбираться!', 3, 3);
