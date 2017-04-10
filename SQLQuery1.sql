CREATE TABLE AudioCodecs (
  Id bigint NOT NULL,
  CodecName varchar(64) DEFAULT NULL,
  CodecValue varchar(255) DEFAULT NULL
);

CREATE TABLE Effect (
  Id bigint NOT NULL,
  EffectFile varchar(255) DEFAULT NULL,
  Version varchar(32) DEFAULT NULL,
  Price float DEFAULT NULL
);

CREATE TABLE Layer (
  Id bigint NOT NULL,
  IdProject bigint DEFAULT NULL,
  Muted_Showed tinyint DEFAULT NULL,
  Blocked varbinary(MAX)
);

CREATE TABLE Project (
  Id bigint NOT NULL,
  IdUser bigint DEFAULT NULL,
  IdVideoCodec bigint DEFAULT NULL,
  IdAudioCodec bigint DEFAULT NULL,
  Width decimal(10,0) DEFAULT NULL,
  Height decimal(10,0) DEFAULT NULL,
  FPS decimal(10,0) DEFAULT NULL,
  Depth varchar(64) DEFAULT NULL,
  Proportions varchar(32) DEFAULT NULL,
  OptimiseFrames tinyint DEFAULT NULL,
  Quality float DEFAULT NULL,
  AudioFreq varchar(32) DEFAULT NULL,
  AudioChanells varchar(16) DEFAULT NULL,
  AudioBitrate varchar(16) DEFAULT NULL
);

CREATE TABLE Row (
  Id bigint NOT NULL,
  IdLayer bigint DEFAULT NULL,
  MaterialFile varchar(255) DEFAULT NULL
);

CREATE TABLE RowEffects (
  Id bigint NOT NULL,
  IdRow bigint DEFAULT NULL,
  IdEffect bigint DEFAULT NULL
);

CREATE TABLE [User] (
  Id bigint NOT NULL,
  Email varchar(64) DEFAULT NULL,
  Surname varchar(32) DEFAULT NULL,
  Name varchar(32) DEFAULT NULL,
  Password varchar(64) DEFAULT NULL
);

CREATE TABLE UsersEffects (
  Id bigint NOT NULL,
  IdUser bigint DEFAULT NULL,
  IdEffect bigint DEFAULT NULL
);

CREATE TABLE VideoCodecs (
  Id bigint NOT NULL,
  CodecName varchar(64) DEFAULT NULL,
  CodecValue varchar(255) DEFAULT NULL
);

ALTER TABLE AudioCodecs
  ADD PRIMARY KEY (Id);

ALTER TABLE Effect
  ADD PRIMARY KEY (Id);

ALTER TABLE Layer
  ADD
	PRIMARY KEY (Id),
	CONSTRAINT FK_IdProject FOREIGN KEY (IdProject) REFERENCES Project (Id);

ALTER TABLE Project
  ADD PRIMARY KEY (Id);

ALTER TABLE Row
  ADD PRIMARY KEY (Id),
  CONSTRAINT FK_IdLayer FOREIGN KEY (IdLayer) REFERENCES Layer (Id);

ALTER TABLE RowEffects
  ADD PRIMARY KEY (Id),
  CONSTRAINT FK_IdEffect FOREIGN KEY (IdEffect) REFERENCES Effect (Id),
  CONSTRAINT FK_IdRow FOREIGN KEY (IdRow) REFERENCES Row (Id);

ALTER TABLE [User]
  ADD PRIMARY KEY (Id);

ALTER TABLE UsersEffects
  ADD PRIMARY KEY (Id),
  CONSTRAINT FK_IdUsersEffectsEffect FOREIGN KEY (IdEffect) REFERENCES Effect (Id),
  CONSTRAINT FK_IdUsersEffectsUser FOREIGN KEY (IdUser) REFERENCES [User] (Id);

ALTER TABLE VideoCodecs
  ADD PRIMARY KEY (Id);

ALTER TABLE Project
  ADD
	  CONSTRAINT FK_IdUser FOREIGN KEY (IdUser) REFERENCES [User] (Id),
	  CONSTRAINT FK_IdAudioCodec FOREIGN KEY (IdAudioCodec) REFERENCES AudioCodecs (Id),
	  CONSTRAINT FK_IdVideoCodec FOREIGN KEY (IdVideoCodec) REFERENCES VideoCodecs (Id);