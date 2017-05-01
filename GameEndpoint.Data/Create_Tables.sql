CREATE TABLE Player
(
	Id INTEGER PRIMARY KEY   AUTOINCREMENT,
	Name varchar (250) not null
);

CREATE TABLE Game
(
	Id INTEGER PRIMARY KEY   AUTOINCREMENT,
	Name varchar (250) not null
);

CREATE TABLE GameResult
(
	Id INTEGER PRIMARY KEY   AUTOINCREMENT,
	PlayerId int not null,
	GameId int not null,
	Win numeric not null,
	Timestamp DATETIMEOFFSET not null,
	FOREIGN KEY (PlayerId) REFERENCES Player(Id),
	FOREIGN KEY (GameId) REFERENCES Game(Id)
);

CREATE INDEX IDX_GamePlayer_GameResult ON GameResult (PlayerId, GameId);
