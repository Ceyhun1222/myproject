CREATE TABLE version (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  key TEXT NOT NULL,
  released_date DATETIME NOT NULL,
  is_last_version INTEGER NOT NULL DEFAULT 1,
  changes_rtf TEXT NOT NULL
);

--***************************************
--***************************************

CREATE TABLE user_group (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  name VARCHAR(200) NOT NULL,
  description TEXT,
  current_version_id INTEGER,
  note TEXT);

--***************************************
--***************************************

CREATE TABLE users (
  id INTEGER PRIMARY KEY AUTOINCREMENT, 
  user_name varchar(40) NOT NULL,
  full_name varchar(300) NOT NULL,
  note TEXT,
  last_downloaded_version INTEGER,
  last_updated_version INTEGER,
  group_id INTEGER REFERENCES user_group(id));
  
--***************************************
--***************************************

CREATE TABLE version_user_group (
   id INTEGER PRIMARY KEY AUTOINCREMENT,
   version_id INTEGER NOT NULL REFERENCES version(id),
   user_group_id INTEGER NOT NULL REFERENCES user_group(id),
   date_time DATETIME NOT NULL DEFAULT (datetime('now', 'localtime')),
   note TEXT);
   
--***************************************
--***************************************

CREATE TABLE user_log(
	id INTEGER PRIMARY KEY AUTOINCREMENT,
	user_id INTEGER REFERENCES users(id) NOT NULL,
	date_time DATETIME NOT NULL,
	message_text TEXT,
	is_read INTEGER NOT NULL DEFAULT 0);

--***************************************
--***************************************

CREATE TABLE common_settings (
   key text,
   value text);
   
--***************************************
--*************************************** 