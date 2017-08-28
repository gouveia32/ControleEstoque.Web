using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;

namespace ControleEstoque.Web
{
    public class PerfilModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

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
                    comando.CommandText = "select count(*) from perfil";
                    ret = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            return ret;
        }

        public static List<PerfilModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<PerfilModel>();

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * from perfil order by nome LIMIT {0},{1}",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
                        });
                    }
                }
            }

            return ret;
        }

        public static List<PerfilModel> RecuperarListaAtivos()
        {
            var ret = new List<PerfilModel>();

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from perfil where ativo=1 order by nome");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
                        });
                    }
                }
            }

            return ret;
        }

        public static PerfilModel RecuperarPeloId(int id)
        {
            PerfilModel ret = null;

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from perfil where (id = @id)";

                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new PerfilModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Ativo = (bool)reader["ativo"]
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
                        comando.CommandText = "delete from perfil where (id = @id)";

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
                        comando.CommandText = "insert into perfil (nome, ativo) values (@nome, @ativo); select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", MySqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);

                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update perfil set nome=@nome, ativo=@ativo where id = @id";

                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", MySqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
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