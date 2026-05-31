-- Script para criar banco de dados para o projeto Vendinha.

-- Se já há um banco de dados com o nome 'vendinha', rode este comando, ele o apagará.
drop database if exists vendinha;

-- Cria o banco de dados 'vendinha'
create database vendinha;

-- Cria table 'clientes'
create table clientes(
	id serial not null,
	nome varchar(100) not null,
	cpf char(11) not null unique,
	data_nascimento date not null,
	status bool not null default true,
	email varchar(100) unique,
	constraint pk_cliente_id primary key (id)
);

-- Cria table 'dividas'
create table dividas(
	id serial not null,
	valor decimal(10, 2) not null,
	situacao bool not null,
	data_criacao timestamp not null default now(),
	data_pagamento timestamp,
	cliente_id int not null,
	constraint fk_divida_cliente foreign key (cliente_id) references clientes(id),
	constraint pk_divida_id primary key (id)
);