using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YazLab11deneme
{
    public partial class GUI1 : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=34.68.27.80;Port=5432;Database=yazlab11;User Id=postgres;Password=root;");
        int girenkullaniciid;
        double haritadanAlinanlat;
        double haritadanAlinanlng;
        public List<PointLatLng> points;
        double kullanicix;
        double kullaniciy;

        double gecicikullanicix;
        double gecicikullaniciy;


        public GUI1()
        {
            
            InitializeComponent();
            cikisYapildi();
            Control.CheckForIllegalCrossThreadCalls = false;
            GMapProviders.GoogleMap.ApiKey = @"AIzaSyDUCJ9n9dc_xBq3Bwbt3Sonk9oqt_na1uA";
            map1.DragButton = MouseButtons.Left;
            map1.MapProvider = GMapProviders.GoogleMap;
            map1.Position = new PointLatLng(40.760, 29.923);
            map1.MinZoom = 1;
            map1.MaxZoom = 100;
            map1.Zoom = 12;
            map1.ShowCenter = false;

            map3.DragButton = MouseButtons.Left;
            map3.MapProvider = GMapProviders.GoogleMap;
            
            map3.ShowCenter = false;
            GUI2 form2 = new GUI2();
            form2.Show();
            listviewgoruntule();
            listviewgoruntulemusteri();
            conn.Open();
            String sorgu = "update kullanici set girildi = false" ;
            NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
            comm2.ExecuteNonQuery();
            conn.Close();

        }

        private void listviewgoruntule()
        {
            conn.Open();
            String sorgu = "select kargoid,musteriid,kargoadi,kargodurumu,kargox,kargoy from kargo order by kargoid asc";
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            NpgsqlDataReader dr = comm.ExecuteReader();


            int kargoid;
            int musteriid;
            String kargoadi;
            String musteriadi;
            bool kargodurumu;
            String kargox;
            String kargoy;

            dr.Read();
            while (dr.Read())
            {
                kargoid = dr.GetInt16(0);
                musteriid = dr.GetInt16(1);
                kargoadi = dr.GetString(2);
                kargodurumu = dr.GetBoolean(3);
                Console.Out.WriteLine("Kargodurum : " + kargodurumu);
                kargox = dr.GetString(4);
                kargoy = dr.GetString(5);
                String a = "lat: " + kargox + " lng: " + kargoy;
                string[] bilgiler = { Convert.ToString( kargoid),Convert.ToString( musteriid), kargoadi,Convert.ToString( kargodurumu),a };
                liste.Items.Add(new ListViewItem(bilgiler));        

            }
            conn.Close();



        }

        private void listviewgoruntulemusteri()
        {
            conn.Open();
            String sorgu = "select musteriid,musteriadi from musteri order by musteriid asc";
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            NpgsqlDataReader dr = comm.ExecuteReader();

            int musteriid;
            String musteriadi;

            dr.Read();
            while (dr.Read())
            {
                musteriid = dr.GetInt16(0);
                musteriadi = dr.GetString(1);
                string[] bilgiler = { Convert.ToString(musteriid) , musteriadi};
                musteriliste.Items.Add(new ListViewItem(bilgiler));
                
            }
            conn.Close();



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            
             //rota
            /*var route = GoogleMapProvider.Instance.GetRoute(points[0], points[1], false, false, 14);
            var r = new GMapRoute(route.Points, "my");
           
            var routes = new GMapOverlay("routes");
            routes.Routes.Add(r);
            map.Overlays.Add(routes);
            double a = route.Distance;
             */
             


            /*
            //marker
             map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            double lat = Convert.ToDouble(bir.Text);
            double longt = Convert.ToDouble(iki.Text);
            map.Position = new PointLatLng(lat,longt);
            map.MinZoom = 1;
            map.MaxZoom = 100;

            //marker ekleme
            PointLatLng point = new PointLatLng(lat,longt);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
            
            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);
            
            map.Overlays.Add(markers);
             */
        }


        private void buton_Click(object sender, EventArgs e)
        {
          /*  map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            double lat = Convert.ToDouble(bir.Text);
            double longt = Convert.ToDouble(iki.Text);
            map.Position = new PointLatLng(lat,longt);
            map.MinZoom = 1;
            map.MaxZoom = 100;

            //marker ekleme
            PointLatLng point = new PointLatLng(lat,longt);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
            
            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);
            
            map.Overlays.Add(markers);
          map.ShowCenter = false;
           */
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* PointLatLng a = new PointLatLng(Convert.ToDouble(bir.Text), Convert.ToDouble(iki.Text));
            points.Add(a);*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //rota
            var route = GoogleMapProvider.Instance.GetRoute(points[0], points[1], false, false, 14);
            var r = new GMapRoute(route.Points, "my");
           
            var routes = new GMapOverlay("routes");
            routes.Routes.Add(r);
            map.Overlays.Add(routes);
        }


        private int basariliGirisMi(String email, String sifre)
        {
            try
            {
                String sb = "SELECT kullaniciid FROM public.kullanici WHERE kullanicimail = '" + email + "' and kullanicisifre = '" + sifre + "'";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand(sb, conn);

                NpgsqlDataReader dr = command.ExecuteReader();

                
                

                if (dr.Read())
                {
                    girenkullaniciid = dr.GetInt16(0);
                    conn.Close();
                    return 1;
                }
               
                else
                {
                    conn.Close();
                    return 0;
                }

            }
            catch (Exception e)
            {
                Console.Out.WriteLine("ERROR: " + e);
            }
            return -1;
        }

        private void girisYapildi()
        {
            tabControl1.TabPages.Add(tabPage6);
            tabControl1.TabPages.Add(tabPage7);
            tabControl1.TabPages.Add(tabPage8);

            l9.Visible = true;
            l10.Visible = true;
            sifredegistir.Visible = true;
            sifredegistir2.Visible = true;
            sifre.Visible = true;
            cikisyap.Visible = true;

            l1.Visible = false;
            l2.Visible = false;
            l3.Visible = false;
            l4.Visible = false;
            l5.Visible = false;
            l6.Visible = false;
            l7.Visible = false;
            l8.Visible = false;

            gMail.Visible = false;
            gSifre.Visible = false;
            kAd.Visible = false;
            kSoyad.Visible = false;
            kMail.Visible = false;
            kSifre.Visible = false;
            kayitol.Visible = false;
            girisyap.Visible = false;
            kayithata.Visible = false;
            girishata.Visible = false;
            kayithata.Text = "";
            girishata.Text = "";

            conn.Open();
            String sorgu = "select kullanicix,kullaniciy from kullanici where kullaniciid = " + girenkullaniciid;
            Console.Out.WriteLine("girenid : " + girenkullaniciid);
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            NpgsqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            String x, y;
            x = dr.GetString(0);
            y = dr.GetString(1);

            Console.Out.WriteLine("x: " + x + " y: " + y);

            kullanicix = Convert.ToDouble(x);
            kullaniciy = Convert.ToDouble(y);

            conn.Close();
            conn.Open();
            sorgu = "update kullanici set girildi = true where kullaniciid = " + girenkullaniciid;
            NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
            comm2.ExecuteNonQuery();
            conn.Close();
            PointLatLng point = new PointLatLng(kullanicix, kullaniciy);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);
            Console.Out.WriteLine("kullanicixy : " + kullanicix + " " + kullaniciy);
            map3.Overlays.Add(markers);
            map3.Position = new PointLatLng(kullanicix, kullaniciy);
            map3.MinZoom = 1;
            map3.MaxZoom = 100;
            map3.Zoom = 12;

        }

        private void cikisYapildi()
        {
            
            tabControl1.TabPages.Remove(tabPage6);
            tabControl1.TabPages.Remove(tabPage7);
            tabControl1.TabPages.Remove(tabPage8);


            l9.Text = "Yeni Şifre";
            l9.Visible = false;
            l10.Visible = false;
            sifredegistir.Visible = false;
            sifredegistir2.Visible = false;
            sifre.Visible = false;
            cikisyap.Visible = false;

            l1.Visible = true;
            l2.Visible = true;
            l3.Visible = true;
            l4.Visible = true;
            l5.Visible = true;
            l6.Visible = true;
            l7.Visible = true;
            l8.Visible = true;

            gMail.Visible = true;
            gSifre.Visible = true;
            kAd.Visible = true;
            kSoyad.Visible = true;
            kMail.Visible = true;
            kSifre.Visible = true;
            kayitol.Visible = true;
            girisyap.Visible = true;
            kayithata.Visible = true;
            girishata.Visible = true;
            yenisifrehata.Text = "";

            conn.Open();
            String sorgu = "update kullanici set girildi = false";
            NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
            comm2.ExecuteNonQuery();
            conn.Close();
            girenkullaniciid = -1;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            String email = gMail.Text;
            String sifre = gSifre.Text;
            bool a = email.Length == 0;
            bool b = sifre.Length == 0;
            if (a & b)
            {
                girishata.ForeColor = System.Drawing.Color.Red; ;
                girishata.Text = "Lütfen Gerekli Yerleri Doldurunuz!";
            }
            else if (a)
            {
                girishata.ForeColor = System.Drawing.Color.Red; ;
                girishata.Text = "Email Adresi Boş Bırakılamaz!";
            }
            else if (b)
            {
                girishata.ForeColor = System.Drawing.Color.Red; ;
                girishata.Text = "Şifre Boş Bırakılamaz!";
            }
            else
            {
                int giris = basariliGirisMi(email, sifre);
                if (giris == 1)
                {
                    girishata.ForeColor = System.Drawing.Color.Green;
                    girishata.Text = "Giriş Başarılı";
                    gMail.Text = "";
                    gSifre.Text = "";
                    
                    girisYapildi();

                }
                else
                {
                    girishata.ForeColor = System.Drawing.Color.Red;
                    girishata.Text = "Hatalı giriş! Tekrar deneyiniz";
                }

            }
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            String yeradi = yerara.Text;
            Console.Out.WriteLine(yeradi);
            map1.SetPositionByKeywords(yeradi);
            map1.Zoom = 12;
            /*
            PointLatLng point = new PointLatLng(lat, longt);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);

            map.Overlays.Add(markers);*/

        }

        private void l9_Click(object sender, EventArgs e)
        {

        }

        private void kayitol_Click(object sender, EventArgs e)
        {
            String email = kMail.Text;
            String sifre = kSifre.Text;
            String ad = kAd.Text;
            String soyad = kSoyad.Text;

            bool a = email.Equals("");
            bool b = sifre.Equals("");
            bool d = ad.Equals("");
            bool f = soyad.Equals("");

            if (a || b || d || f)
            {
                kayithata.ForeColor = System.Drawing.Color.Red;
                kayithata.Text = "Lütfen Gerekli Yerleri Doldurunuz!";
            }
            else
            {
                int kayit = kullaniciKayit(email);
                Console.Out.WriteLine(kayit);
                if (kayit == 1)
                {
                    kayitEkle(ad, soyad, email, sifre);
                    kayithata.ForeColor = System.Drawing.Color.Green;
                    kayithata.Text  = "Kayıt Başarılı";
                }
                else
                {
                    kayithata.ForeColor = System.Drawing.Color.Red;
                    kayithata.Text = "Bu email daha önce kullanıldı!";
                }

            }
        }

        private int kullaniciKayit(String email)
        {
            try
            {
                String sorgu = "SELECT * FROM public.kullanici WHERE kullanicimail = '" + email + "'";
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand(sorgu, conn);

                NpgsqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    conn.Close();
                    return 0;
                }
                else
                {
                    conn.Close();
                    return 1;
                }

            }
            catch (Exception e)
            {
                conn.Close();
                Console.Out.WriteLine("ERROR: " + e);
            }
            conn.Close();
            return -1;
        }

        private void kayitEkle(String ad, String soyad, String email, String sifre)
        {
            try
            {
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand("SELECT MAX(kullaniciID) FROM public.kullanici", conn);

                
                Int16 count = (Int16)comm.ExecuteScalar();
                Console.Out.WriteLine("count: "+count);
                
                int kayitSayisi = (int)count;
                kayitSayisi++;
                Console.Out.WriteLine("kayit: " + kayitSayisi);
                String sorgu = "INSERT INTO public.kullanici(kullaniciid,kullaniciadi,kullanicisoyadi,kullanicimail,kullanicisifre) VALUES ("
                        + kayitSayisi + ",'" + ad + "','" + soyad + "','" + email + "','" + sifre + "')";
                
                NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
                int a = comm2.ExecuteNonQuery();
                Console.Out.WriteLine(a + " a değeri ");
                kMail.Text = "";
                kAd.Text = "";
                kSoyad.Text = "";
                kSifre.Text = "";
                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
                Console.Out.WriteLine("ERROR: " + e);
            }
        }

        private void cikisyap_Click(object sender, EventArgs e)
        {
            girenkullaniciid = 0;
            cikisYapildi();
        }

        private void sifre_Click(object sender, EventArgs e)
        {
            String sifre1 = sifredegistir.Text;
            String sifre2 = sifredegistir2.Text;

            if (sifre1.Equals(sifre2))
            {
                try
                {
                    String sorgu = "UPDATE public.kullanici set kullanicisifre =  '" + sifre1 + "' where kullaniciid = " + girenkullaniciid;
                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);

                    comm.ExecuteNonQuery();
                    yenisifrehata.ForeColor = System.Drawing.Color.Green;
                    yenisifrehata.Text = "Şifre başarıyla değiştirildi!";
                    sifredegistir.Text = "";
                    sifredegistir2.Text = "";
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("ERROR: " + ex);
                }
                    
            } else
            {
                yenisifrehata.ForeColor = System.Drawing.Color.Red;
                yenisifrehata.Text = "Şifreler aynı olmalıdır!";
            }
        }

        private void map1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (map1.Overlays.Count > 0)
                {
                    map1.Overlays.RemoveAt(0);
                    map1.Refresh();
                }
                var point = map1.FromLocalToLatLng(e.X, e.Y);
                haritadanAlinanlat = point.Lat;
                haritadanAlinanlng = point.Lng;
                Console.Out.WriteLine("lat: " + haritadanAlinanlat + " lng: " + haritadanAlinanlng);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

                GMapOverlay markers = new GMapOverlay("markers");
                markers.Markers.Add(marker);
                
                map1.Overlays.Add(markers);

            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            String kargoadi = kargoekleadi.Text;
            String musteriid = kargoeklemusteri.Text;
            bool a = kargoadi.Equals("");
            bool b = musteriid.Equals("");

            if (a & b)
            {
                kargoEkleHata.ForeColor = System.Drawing.Color.Red;
                kargoEkleHata.Text = "Gerekli Yerleri Doldurunuz!";
            }
            else if (a)
            {
                kargoEkleHata.ForeColor = System.Drawing.Color.Red;
                kargoEkleHata.Text = "Kargo Adını Giriniz!";
            }
            else if (b)
            {
                kargoEkleHata.ForeColor = System.Drawing.Color.Red;
                kargoEkleHata.Text = "Müşteri ID Giriniz!";
            }
            else
            {
                conn.Open();
                String sorgu = "select * from musteri where musteriid = " + musteriid;
                try
                {
                    NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
                    NpgsqlDataReader dr = comm.ExecuteReader();
                    
                    if (dr.Read())
                    {
                        if(haritadanAlinanlat == 0 && haritadanAlinanlng == 0)
                        {
                            kargoEkleHata.ForeColor = System.Drawing.Color.Red;
                            kargoEkleHata.Text = "Yer Seçiniz!";
                            conn.Close();
                        }
                        else
                        {
                            int kargoid;
                             sorgu = "select max(kargoid) from public.kargo";
                            conn.Close();
                            conn.Open();
                            NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
                            NpgsqlDataReader dr2 = comm2.ExecuteReader();
                            dr2.Read();
                            kargoid = dr2.GetInt16(0);
                            kargoid++;
                            Console.Out.WriteLine("lat: " + Math.Round(decimal.Parse(Convert.ToString (haritadanAlinanlat)), 3) + " lng : " + Math.Round(decimal.Parse(Convert.ToString(haritadanAlinanlng)), 3));

                            sorgu = "INSERT INTO public.kargo(kargodurumu,kargoid,kargoadi,musteriid,kargox,kargoy) VALUES (false,"
                                + kargoid + ",'" + kargoadi + "'," + musteriid + ",'"+Convert.ToString( haritadanAlinanlat) + "','"+ Convert.ToString(haritadanAlinanlng) + "')";
                            conn.Close();
                            conn.Open();
                            NpgsqlCommand comm3 = new NpgsqlCommand(sorgu, conn);

                            comm3.ExecuteNonQuery();
                            conn.Close();
                            kargoEkleHata.ForeColor = System.Drawing.Color.Green;
                            kargoEkleHata.Text = "Kargo Eklendi!";
                            kargoekleadi.Text = "";
                            kargoeklemusteri.Text = "";
                            liste.Items.Clear();
                            listviewgoruntule();
                        }
                        

                    }
                    else
                    {
                        kargoEkleHata.ForeColor = System.Drawing.Color.Red;
                        kargoEkleHata.Text="Müşteri Bulunamıyor!";
                    }
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("ERROR: " + ex);
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String musteriadi = musteriekle.Text;

            bool a = musteriadi.Equals("");

            if (a)
            {
                musteriEkleHata.ForeColor = System.Drawing.Color.Red;
                musteriEkleHata.Text = "Müşteri Adını Giriniz!";
            }
            else
            {
                conn.Open();
                String sorgu;
                try
                {
                    
                    int musteriid;
                    sorgu = "select max(musteriid) from public.musteri";
                    NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
                    NpgsqlDataReader dr = comm.ExecuteReader();
                    dr.Read();
                    musteriid = dr.GetInt16(0);
                    musteriid++;
                    conn.Close();
                    conn.Open();
                    sorgu = "INSERT INTO public.musteri(musteriadi, musteriid)"
                            + " VALUES ('"
                            + musteriadi + "'," + musteriid + ")";
                    NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
                    comm2.ExecuteNonQuery();
                    conn.Close();
                    musteriEkleHata.ForeColor = System.Drawing.Color.Green;
                    musteriEkleHata.Text = "Müşteri Eklendi!";
                    musteriekle.Text = "";

                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("ERROR: " + ex);
                }

            }
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

            kullanicix = gecicikullanicix;
            kullaniciy = gecicikullaniciy;

            String sorgu = "UPDATE public.kullanici set kullanicix =  '" + Convert.ToString( kullanicix) + "', kullaniciy = '" +Convert.ToString( kullaniciy) + "' where kullaniciid = " + girenkullaniciid;
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);

            comm.ExecuteNonQuery();
            conn.Close();
        }

        private void map3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (map3.Overlays.Count > 0)
                {
                    map3.Overlays.RemoveAt(0);
                    map3.Refresh();
                }
                var point = map3.FromLocalToLatLng(e.X, e.Y);
                gecicikullanicix = point.Lat;
                gecicikullaniciy = point.Lng;
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

                GMapOverlay markers = new GMapOverlay("markers");
                markers.Markers.Add(marker);

                map3.Overlays.Add(markers);

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            String yeradi = yeraralokasyon.Text;
            map3.SetPositionByKeywords(yeradi);
            map3.Zoom = 12;
        }

        private void button10_Click(object sender, EventArgs e)
        {


            String kargoid = kargosil.Text;
            conn.Open();
            String sorgu = "delete from kargo where kargoid = " + kargoid;
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            comm.ExecuteNonQuery();
            conn.Close();
            liste.Items.Clear();
            listviewgoruntule();
            kargosilhata.ForeColor = System.Drawing.Color.Green;
            kargosilhata.Text = "Kargo silindi!";
            kargosil.Text = "";

        }

        private void button11_Click(object sender, EventArgs e)
        {
            String kargoid = kargoteslim.Text;
            conn.Open();
            String sorgu = "update kargo set kargodurumu = true where kargoid = " + kargoid;
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            comm.ExecuteNonQuery();
            conn.Close();
            liste.Items.Clear();
            listviewgoruntule();
            kargoteslimhata.ForeColor = System.Drawing.Color.Green;
            kargoteslimhata.Text = "Kargo teslim edildi!";
            kargoteslim.Text = "";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            String kargoid = kargoteslimiptal.Text;
            conn.Open();
            String sorgu = "update kargo set kargodurumu = false where kargoid = " + kargoid;
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            comm.ExecuteNonQuery();
            conn.Close();
            liste.Items.Clear();
            listviewgoruntule();
            kargoteslimiptalhata.ForeColor = System.Drawing.Color.Green;
            kargoteslimiptalhata.Text = "Kargo teslimi iptal edildi!";
            kargoteslimiptal.Text = "";
        }
    }
    
}
