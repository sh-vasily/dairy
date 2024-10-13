create table if not exists poems(
  id integer primary key autoincrement,
  content text not null,
  loaded_at datetime  
);