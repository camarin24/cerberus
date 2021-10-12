create database intecgra_cerberus;

create schema auth;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp"

create table auth.application(
                                 application_id uuid DEFAULT uuid_generate_v4 (),
                                 name VARCHAR(250) NOT null,
                                 PRIMARY KEY (application_id)
);


create table auth.client(
                            client_id uuid primary key not null DEFAULT uuid_generate_v4 (),
                            name VARCHAR(250) NOT null
);

create table auth.user(
                          user_id uuid primary key DEFAULT uuid_generate_v4 (),
                          client_id uuid not null,
                          name VARCHAR(250),
                          email VARCHAR(250),
                          picture VARCHAR,
                          
                          foreign key (client_id) references auth.client(client_id)
);

ALTER TABLE auth."user" ADD password text NOT NULL;
ALTER TABLE auth."user" ADD salt text NOT NULL;


create table auth.permission(
                                permission_id serial not null primary key,
                                application_id uuid not null,
                                name varchar(250) not null,
                                description varchar(250),
                                foreign key (application_id) references auth.application(application_id)
);


create table auth.user_permission(
                                     user_permission_id serial not null primary key,
                                     permission_id int not null,
                                     user_id uuid not null,
                                     foreign key (permission_id) references auth.permission(permission_id),
                                     foreign key (user_id) references auth.user(user_id)
);


create table auth.client_application(
                                        client_application_id serial not null primary key,
                                        application_id uuid not null,
                                        client_id uuid not null,
                                        foreign key (application_id) references auth.application(application_id),
                                        foreign key (client_id) references auth.client(client_id)
);


SELECT uuid_generate_v4();
