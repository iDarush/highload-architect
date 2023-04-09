using FluentMigrator;

namespace Architect.Migrator.Migrations;

[Migration(0001L)]
public class AddUser : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists users
            (
                id uuid DEFAULT uuid_generate_v4() constraint users_pk primary key,
                first_name text not null,
                last_name text not null,
                age smallint not null,
                gender smallint not null,
                hobby text not null,
                city text not null,
                password_hash text not null
            );

            DO $$BEGIN
                IF NOT EXISTS (select 1 from pg_type where typname = 'user_v1') THEN
                    create type user_v1 as
                    (
                        id uuid,
                        first_name text,
                        last_name text,
                        age smallint,
                        gender smallint,
                        hobby text,
                        city text,
                        password_hash text
                    );
                END IF;
            END$$;

            comment on table users is 'Пользователи';
            comment on column users.id is 'Идентификатор';
            comment on column users.first_name is 'Имя';
            comment on column users.last_name is 'Фамилия';
            comment on column users.age is 'Возраст';
            comment on column users.gender is 'Пол';
            comment on column users.hobby is 'Увлечения';
            comment on column users.city is 'Город';
            comment on column users.password_hash is 'Хэш пароля';
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            drop type if exists user_v1;
            drop table if exists users;
        ");
    }
}
