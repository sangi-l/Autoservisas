using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
        public string Name { get; set; }

        [DisplayName("Tūris")]
        [Required]
        public double Displacement { get; set; }

        [DisplayName("Pagaminimo Metai")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime finishedDate { get; set; }

        [DisplayName("Meistro ID")]
        [Required]
        public int MechanicID { get; set; }

        [DisplayName("Ar parinkti tik orginalias dalis?")]
        [Required]
        public bool OriginalParts { get; set; }

        [DisplayName("Ar parinkti tik kokybiškiausias dalis?")]
        [Required]
        public bool QualityParts { get; set; }

        [DisplayName("Ar parinkti detales kurios kainuoja mažiau nei vidutinė jų kainą?")]
        [Required]
        public bool AvgPrice { get; set; }

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
                        Name = Convert.ToString(dr["modelis"]),
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

        public Work GetReservation(int id)
        {
            Work reservation = new Work();
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
                int rid = Convert.ToInt32(dr["id_rezervacija"]);
                if (rid == id)
                {
                    reservation = new Work
                    {
                        ReservationID = rid,
                        Data = Convert.ToDateTime(dr["data"]),
                        Make = Convert.ToString(dr["marke"]),
                        Name = Convert.ToString(dr["modelis"]),
                        Displacement = Convert.ToDouble(dr["turis"]),
                        MakeDate = Convert.ToDateTime(dr["pagaminimo_metai"]).Date,
                        FuelType = Convert.ToString(dr["kuro_tipas"]),
                        Plate = Convert.ToString(dr["valstybinis_nr"]),
                        Price = Convert.ToDouble(dr["darbo_kaina"]),
                        Sum = Convert.ToDouble(dr["suma"]),
                        finishedDate = Convert.ToDateTime(dr["pabaigos_data"]),
                    };
                }
            }

            return reservation;
        }

        public bool UpdateDetails(Work model)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateReservationDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_rezervacija", model.ReservationID);
            cmd.Parameters.AddWithValue("@darbo_kaina", model.Price);
            cmd.Parameters.AddWithValue("@pabaigos_data", model.finishedDate);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public string FindSymptoms(int id)
        {
            string symptom = "";
            connection();

            SqlCommand cmd = new SqlCommand("GetSymptomsIdFromReservation", con);
            cmd.Parameters.AddWithValue("@id_rezervacija", id);

            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            int sid = -1;
            foreach (DataRow dr in dt.Rows)
            {
                sid = Convert.ToInt32(dr["fk_simptomasid"]);
            }


            SqlCommand cmd2 = new SqlCommand("GetSymptomsDescFromID", con);
            cmd2.Parameters.AddWithValue("@id", sid);

            cmd2.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();


            con.Open();
            sd2.Fill(dt2);
            con.Close();

            foreach (DataRow dr in dt2.Rows)
            {
                symptom = Convert.ToString(dr["aprasymas"]);
            }

            return symptom;
        }


        public List<Part> FilterBySymptom(string symptom)
        {
            List<Part> parts = new List<Part>();
            connection();

            SqlCommand cmd = new SqlCommand("GetPartDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                string smp = Convert.ToString(dr["zymos"]);
                if (smp == symptom)
                {
                    parts.Add(
                        new Part
                        {
                            PartID = Convert.ToInt32(dr["id_detale"]),
                            Price = Convert.ToDouble(dr["kaina"]),
                            Name = Convert.ToString(dr["pavadinimas"]),
                            Code = Convert.ToInt32(dr["detales_kodas"]),
                            Tags = smp,
                            Picture = Convert.ToString(dr["nuotrauka"]),
                            Details = Convert.ToString(dr["aprasymas"]),
                            Ammount = Convert.ToInt32(dr["likutis"]),
                            Quality = Convert.ToInt32(dr["kokybe"]),
                            Category = Convert.ToString(dr["kategorija"]),
                            Originallity = Convert.ToBoolean(dr["orginalumas"])
                        }
                        );
                }
            }

            return parts;
        }

        public double AveragePrice(List<Part> parts)
        {
            double avg = 0;

            foreach(var item in parts)
            {
                avg += item.Price;
            }

            avg = avg / parts.Count;

            return avg;
        }
    }
}