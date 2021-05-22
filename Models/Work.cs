﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Autoservisas.Models
{
    public class Work
    {
        [DisplayName("ID")]
        public int ReservationID { get; set; }

        [DisplayName("Data")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [DisplayName("Marke")]
        [Required]
        public string Make { get; set; }

        [DisplayName("Modelis")]
        [Required]
        public string Model { get; set; }

        [DisplayName("Tūris")]
        [Required]
        public double Displacement { get; set; }

        [DisplayName("Pagaminimo Metai")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime MakeDate { get; set; }

        [DisplayName("Kuro tipas")]
        [Required]
        public string FuelType { get; set; }

        [DisplayName("Valstybinis nr.")]
        [Required]
        public string Plate { get; set; }

        [DisplayName("Darbo kaina")]
        [Required]
        public double Price { get; set; }

        [DisplayName("Suma")]
        [Required]
        public double Sum { get; set; }

        [DisplayName("Pabaigos Data")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime finishedDate { get; set; }

        [DisplayName("Meistro ID")]
        [Required]
        public int MechanicID { get; set; }

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Database"].ToString();
            con = new SqlConnection(constring);
        }

        public List<Work> GetWork(int id)
        {
            List<Work> work = new List<Work>();
            connection();

            SqlCommand cmd = new SqlCommand("GetReservationDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                int mid = Convert.ToInt32(dr["fk_meistrasid"]);
                if (mid == id)
                {
                    work.Add(
                    new Work
                    {
                        ReservationID = Convert.ToInt32(dr["id_rezervacija"]),
                        Data = Convert.ToDateTime(dr["data"]),
                        Make = Convert.ToString(dr["marke"]),
                        Model = Convert.ToString(dr["modelis"]),
                        Displacement = Convert.ToDouble(dr["turis"]),
                        MakeDate = Convert.ToDateTime(dr["pagaminimo_metai"]),
                        FuelType = Convert.ToString(dr["kuro_tipas"]),
                        Plate = Convert.ToString(dr["valstybinis_nr"]),
                        Price = Convert.ToDouble(dr["darbo_kaina"]),
                        Sum = Convert.ToDouble(dr["suma"]),
                        finishedDate = Convert.ToDateTime(dr["pabaigos_data"]),
                        MechanicID = mid
                    }); ;
                }
            }

            return work;
        }
    }
}