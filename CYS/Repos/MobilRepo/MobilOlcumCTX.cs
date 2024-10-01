using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class MobilOlcumCTX
	{
		private readonly string _connectionString = "Server=localhost;Database=CYS;User Id=root;Password=Muhamm3d!1;";

		// MobilOlcum Listesi
		public List<MobilOlcum> MobilOlcumList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection(_connectionString))
			{
				return connection.Query<MobilOlcum>(sorgu, param).ToList();
			}
		}

		// Tek bir MobilOlcum getir
		public MobilOlcum MobilOlcumTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection(_connectionString))
			{
				return connection.Query<MobilOlcum>(sorgu, param).FirstOrDefault();
			}
		}

		// Yeni MobilOlcum ekle
		public int MobilOlcumEkle(MobilOlcum olcum)
		{
			using (var connection = new MySqlConnection(_connectionString))
			{
				var sql = "INSERT INTO mobilolcum (rfid, weight, cihazid, amacid, hayvanid, tarih) VALUES (@Rfid, @Weight, @CihazId, @AmacId, @HayvanId, @Tarih)";
				return connection.Execute(sql, olcum);
			}
		}

		// Mevcut bir MobilOlcum güncelle
		public int MobilOlcumGuncelle(MobilOlcum olcum)
		{
			using (var connection = new MySqlConnection(_connectionString))
			{
				var sql = "UPDATE mobilolcum SET rfid = @Rfid, weight = @Weight, cihazid = @CihazId, amacid = @AmacId, hayvanid = @HayvanId, tarih = @Tarih WHERE id = @Id";
				return connection.Execute(sql, olcum);
			}
		}
	}
}
