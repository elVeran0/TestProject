INSERT INTO cities (name)
VALUES 
('Москва'), ('Санкт-Петербург'), ('Пенза'), ('Екатеринбург'), 
('Калининград'), ('Сочи'), ('Владивосток'), ('Казань');

INSERT INTO airlines (name)
VALUES 
('S7 Airlines'), ('Nordwind Airlines'), ('Аэрофлот'), ('Авиокомпания "Победа"'),
('Ural Airlines'), ('Nordstar Airlines'), ('Red Wings Airlines'), ('UTair');

INSERT INTO hotels (name, city_id)
VALUES 
('Гостиница Вега Измайлово', 1), ('Гостиница Метрополь', 1), 
('АЗИМУТ Отель Санкт-Петербург', 2), ('Solo Sokos Hotel Palace Bridge', 2), 
('HELIOPARK Residence Hotel', 3), ('Гостиница Для Вас', 3), 
('Отель Высоцкий', 4), ('Angelo by Vienna House Екатеринбург', 4), 
('Гостиница Калининград', 5), ('Гостиница Турист', 5), 
('Родина Гранд Отель и Спа', 6), ('Гранд Отель "Жемчужина"', 6), 
('AZIMUT Hotel Vladivostok', 7), ('Жемчужина Отель', 7),
('Отель Корстон Роял Казань', 8), ('Отель Мираж', 8);

INSERT INTO air_tickets_base (depart, arrive, from_city_id, to_city_id, airlines_id, price)
VALUES 
('2017.12.19 6:30',  '2017.12.19 7:50',  3, 1, 1, 3325), 
('2017.12.22 17:30', '2017.12.22 19:00', 1, 3, 1, 3450), 
('2017.12.19 15:30', '2017.12.19 16:55', 1, 8, 1, 4164), 
('2017.12.22 9:15',  '2017.12.22 10:35', 8, 1, 1, 5103), 
('2017.12.19 17:00', '2017.12.19 18:25', 1, 2, 8, 1430), 
('2017.12.22 13:00', '2017.12.22 14:20', 2, 1, 8, 1550), 
('2017.12.19 11:10', '2017.12.19 12:35', 1, 2, 4, 1432),
('2017.12.22 21:05', '2017.12.22 22:30', 2, 1, 4, 1524),
('2017.12.19 8:15',  '2017.12.19 10:35', 1, 6, 4, 1320),
('2017.12.22 7:25',  '2017.12.22 9:40',  6, 1, 4, 1340),
('2017.12.19 10:50', '2017.12.19 13:10', 1, 6, 8, 1430),
('2017.12.22 6:00',  '2017.12.22 8:20',  6, 1, 8, 1540),
('2017.12.19 6:15',  '2017.12.19 7:10',  4, 2, 3, 5557),
('2017.12.22 10:35', '2017.12.22 15:15', 2, 4, 3, 4535), 
('2017.12.20 6:30',  '2017.12.19 7:50',  3, 1, 1, 3325), 
('2017.12.23 17:30', '2017.12.23 19:00', 1, 3, 1, 3450), 
('2017.12.20 15:30', '2017.12.20 16:55', 1, 8, 1, 4164), 
('2017.12.23 9:15',  '2017.12.23 10:35', 8, 1, 1, 5103), 
('2017.12.20 17:00', '2017.12.20 18:25', 1, 2, 8, 1430), 
('2017.12.23 13:00', '2017.12.23 14:20', 2, 1, 8, 1550), 
('2017.12.20 11:10', '2017.12.20 12:35', 1, 2, 4, 1432),
('2017.12.23 21:05', '2017.12.23 22:30', 2, 1, 4, 1524),
('2017.12.20 8:15',  '2017.12.20 10:35', 1, 6, 4, 1320),
('2017.12.23 7:25',  '2017.12.23 9:40',  6, 1, 4, 1340),
('2017.12.20 10:50', '2017.12.20 13:10', 1, 6, 8, 1430),
('2017.12.23 6:00',  '2017.12.23 8:20',  6, 1, 8, 1540),
('2017.12.20 6:15',  '2017.12.20 7:10',  4, 2, 3, 5557),
('2017.12.23 10:35', '2017.12.23 15:15', 2, 4, 3, 4535),
('2017.12.21 6:30',  '2017.12.19 7:50',  3, 1, 1, 3325), 
('2017.12.24 17:30', '2017.12.24 19:00', 1, 3, 1, 3450), 
('2017.12.21 15:30', '2017.12.21 16:55', 1, 8, 1, 4164), 
('2017.12.24 9:15',  '2017.12.24 10:35', 8, 1, 1, 5103), 
('2017.12.21 17:00', '2017.12.21 18:25', 1, 2, 8, 1430), 
('2017.12.24 13:00', '2017.12.24 14:20', 2, 1, 8, 1550), 
('2017.12.21 11:10', '2017.12.21 12:35', 1, 2, 4, 1432),
('2017.12.24 21:05', '2017.12.24 22:30', 2, 1, 4, 1524),
('2017.12.21 8:15',  '2017.12.21 10:35', 1, 6, 4, 1320),
('2017.12.24 7:25',  '2017.12.24 9:40',  6, 1, 4, 1340),
('2017.12.21 10:50', '2017.12.21 13:10', 1, 6, 8, 1430),
('2017.12.24 6:00',  '2017.12.24 8:20',  6, 1, 8, 1540),
('2017.12.21 6:15',  '2017.12.21 7:10',  4, 2, 3, 5557),
('2017.12.24 10:35', '2017.12.24 15:15', 2, 4, 3, 4535);

INSERT INTO carts (user_id)
VALUES (1);