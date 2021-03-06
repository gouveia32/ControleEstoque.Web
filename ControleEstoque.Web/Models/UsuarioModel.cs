﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Informe o senha")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o perfil")]
        public int IdPerfil { get; set; }

        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
            UsuarioModel ret = null;

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from usuario where login=@login and senha=@senha";

                    comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                    comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Login = (string)reader["login"],
                            Senha = (string)reader["senha"],
                            Nome = (string)reader["nome"],
                            IdPerfil = (int)reader["id_perfil"]
                        };
                    }
                }
            }

            return ret;
        }

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from usuario";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static List<UsuarioModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<UsuarioModel>();

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * from usuario order by nome LIMIT {0},{1}",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Login = (string)reader["login"],
                            IdPerfil = (int)reader["id_perfil"]
                        });
                    }
                }
            }

            return ret;
        }

        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from usuario where (id = @id)";

                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Login = (string)reader["login"],
                            IdPerfil = (int)reader["id_perfil"]
                        };
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new MySqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new MySqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from usuario where (id = @id)";

                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into usuario (nome, login, senha, id_perfil) values (@nome, @login, @senha, @id_perfil); select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                        comando.Parameters.Add("@id_perfil", MySqlDbType.Int32).Value = this.IdPerfil;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText =
                            "update usuario set nome=@nome, login=@login, id_perfil=@id_perfil" +
                            (!string.IsNullOrEmpty(this.Senha) ? ", senha=@senha" : "") +
                            " where id = @id";

                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@id_perfil", MySqlDbType.Int32).Value = this.IdPerfil;

                        if (!string.IsNullOrEmpty(this.Senha))
                        {
                            comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                        }

                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }

            return ret;
        }
    }
}