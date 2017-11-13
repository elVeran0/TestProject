CREATE TABLE cities (
  id serial,
  name text,
  PRIMARY KEY (id)
);

CREATE TABLE airlines (
  id serial,
  name text,
  PRIMARY KEY (id)
);

CREATE TABLE hotels (
  id serial,
  name text,
  city_id integer REFERENCES cities(id),
  address text DEFAULT 'Информация отсутствует',
  PRIMARY KEY (id)
);

CREATE TABLE air_tickets (
  id serial,
  depart timestamptz,
  arrive timestamptz,
  from_city_id integer REFERENCES cities(id),
  to_city_id integer REFERENCES cities(id),
  airlines_id integer REFERENCES airlines(id),
  price numeric(15, 4) CHECK (price > 0),
  PRIMARY KEY (id)
  );

CREATE TABLE air_tickets_base (
  id serial,
  depart timestamptz,
  arrive timestamptz,
  from_city_id integer REFERENCES cities(id),
  to_city_id integer REFERENCES cities(id),
  airlines_id integer REFERENCES airlines(id),
  price numeric(15, 4) CHECK (price > 0),
  PRIMARY KEY (id)
  );

CREATE TABLE hotel_orders (
  id serial,
  check_in timestamptz,
  check_out timestamptz,
  hotel_id integer REFERENCES hotels(id),
  price numeric(15, 4) CHECK (price > 0),
  PRIMARY KEY (id)
  );

CREATE TABLE orders (
  id serial,
  air_ticket_id integer REFERENCES air_tickets(id),
  hotel_order_id integer REFERENCES hotel_orders(id),
  PRIMARY KEY (id)
);

CREATE TABLE carts (
  id serial,
  user_id integer,--REFERENCES users(id),
  datetime timestamptz DEFAULT now(),
  PRIMARY KEY (id)
);

CREATE TABLE cart_items (
  id serial,
  cart_id integer REFERENCES carts(id),
  order_id integer REFERENCES orders(id),
  PRIMARY KEY (id)
);