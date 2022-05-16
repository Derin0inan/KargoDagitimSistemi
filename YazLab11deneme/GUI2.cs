using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace YazLab11deneme
{
   
        public partial class GUI2 : Form
        {
        NpgsqlConnection conn = new NpgsqlConnection("Server=34.68.27.80;Port=5432;Database=yazlab11;User Id=postgres;Password=root;");
        double konumx = 0, konumy = 0;
        int kargosayisi;
        int[] index;
        double[][] xy;
        int[] yol;
        public List<PointLatLng> points;
        GMapOverlay markers;

        public void kargoYenile()
        {
            for(int i = 0; ; i++)
            {
                //haritayitemizle();
                //kargolariGoster();
                Thread.Sleep(500);
            }
                
            
        }

        private void kargolariGoster()
        {
            conn.Open();
            String x, y;

            haritayitemizle();


            String sorgu = "select kargox,kargoy from kargo where kargodurumu = false";
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            NpgsqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {

                double lat, lng;
                
                 x = dr.GetString(0);
                 y = dr.GetString(1);
                lat = Convert.ToDouble(x);
                lng = Convert.ToDouble(y);
                PointLatLng point = new PointLatLng(lat, lng);
                //Console.Out.WriteLine("KONUMLAR: " + lat + " " + lng);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

                 markers = new GMapOverlay("markers");
                markers.Markers.Add(marker);
                
                map.Overlays.Add(markers);

            }
            conn.Close();


            
            conn.Open();
            sorgu = "select kullanicix,kullaniciy from kullanici where girildi = true";
            NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
            NpgsqlDataReader dr2 = comm2.ExecuteReader();
            if (dr2.Read())
            {
                x = dr2.GetString(0);
                y = dr2.GetString(1);
                konumx = Convert.ToDouble(x);
                konumy = Convert.ToDouble(y);
            }
            conn.Close();
            if (konumx != 0 && konumy != 0)
            {
                PointLatLng point1 = new PointLatLng(konumx, konumy);
                GMapMarker marker1 = new GMarkerGoogle(point1, GMarkerGoogleType.blue_dot);

                markers = new GMapOverlay("markers");
                markers.Markers.Add(marker1);
                //Console.Out.WriteLine("KONUMLAR lokasyon: " + konumx + " " + konumy);

                map.Overlays.Add(markers);
            }

            



        }


        public GUI2()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            GMapProviders.GoogleMap.ApiKey = @"AIzaSyDUCJ9n9dc_xBq3Bwbt3Sonk9oqt_na1uA";
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.Position = new PointLatLng(40.760, 29.923);
            map.MinZoom = 1;
            map.MaxZoom = 100;
            map.Zoom = 12;
            map.ShowCenter = false;
            conn.Open();
            String sorgu = "select count(kargoid) from kargo";
            NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
            NpgsqlDataReader dr = comm.ExecuteReader();
            dr.Read();
            kargosayisi = dr.GetInt16(0);
            conn.Close();
            primOlmadıgıIcinYapiyorum();
            Thread t = new Thread(new ThreadStart(kargoYenile));
            t.Start();
        }

             /*
             //rota
            var route = GoogleMapProvider.Instance.GetRoute(points[0], points[1], false, false, 14);
            var r = new GMapRoute(route.Points, "my");

            var routes = new GMapOverlay("routes");
            routes.Routes.Add(r);
            map.Overlays.Add(routes);
            double a = route.Distance;

            */
        private void prim()
        {
            double enky=999999999;
            
            if (konumx != 0 && konumy != 0)
            {
                conn.Open();
                String sorgu = "select kargox,kargoy,kargoid from kargo order by kargoid asc";
                NpgsqlCommand comm2 = new NpgsqlCommand(sorgu, conn);
                NpgsqlDataReader dr2 = comm2.ExecuteReader();
                dr2.Read();
                dr2.Read();
                conn.Close();

                int[] idler = new int[kargosayisi]; 
                xy = new double[kargosayisi][];
                yol = new int[kargosayisi+1];
                index = new int[kargosayisi];
                double[][] komsulukmatrisi = new double[kargosayisi][];
                xy[0][0] = new double();
                xy[0][0] = konumx;

                xy[0][1] = new double();
                xy[0][1] = konumy;

                PointLatLng point2;
                PointLatLng point1;

                for (int i = 1; i<kargosayisi ; i++)
                {
                    String x, y;
                    double lat, lng;
                    x = dr2.GetString(0);
                    y = dr2.GetString(1);
                    lat = Convert.ToDouble(x);
                    lng = Convert.ToDouble(y);

                    xy[i][0] = lat;
                    xy[i][1] = lng;
                    idler[i] = dr2.GetInt32(2);
                    dr2.Read();  

                }
                index[0] = 0;

                for(int i = 0; i < kargosayisi; i++)
                {
                    point1 = new PointLatLng(xy[i][0], xy[i][1]);
                    for (int j = 0; j < kargosayisi ; j++)
                    {
                        if(j==i)
                        {
                            komsulukmatrisi[i][i] = 9999999;
                        }
                        else
                        {
                            point2 = new PointLatLng(xy[j][0], xy[j][1]);
                            var route = GoogleMapProvider.Instance.GetRoute(point1, point2, false, false, 14);
                            var r = new GMapRoute(route.Points, "my");
                            var routes = new GMapOverlay("routes");
                            routes.Routes.Add(r);
                            //map.Overlays.Add(routes);
                            double a = route.Distance;
                            komsulukmatrisi[i][j] = a;
                        }
                        
                    }
                }
                int ind=0,ind2=0;
                yol[0] = 0;
                for(int i = 0; i < kargosayisi; i++)
                {
                    for(int j = 0; j < kargosayisi; j++)
                    {
                        if (enky > komsulukmatrisi[ind][j] && !kullanildimi(j))
                        {
                            enky = komsulukmatrisi[ind][j];
                            ind2 = j;
                        }
                        
                    }
                    
                    yol[i+1] =ind2;
                    ind = ind2;
                    
                }



                for (int i = 0; i < yol.Length; i++)
                {
                    Console.Out.WriteLine("YOL: " + yol[i]);

                        
                }




            }
        } 


        private bool kullanildimi(int a)
        {
            for(int i = 0; i < yol.Length; i++)
            {
                if (yol[i] == a)
                    return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kargolariGoster();
            //primOlmadıgıIcinYapiyorum();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            haritayitemizle();

        }

        private void haritayitemizle()
        {
for (int i = 0; i < map.Overlays.Count + 1; i++)
            {
                if (map.Overlays.Count > 0)
                {
                    Console.Out.WriteLine("HEEEEEEEEYYYYYYY!!!!!! " + map.Overlays.Count);
                    map.Overlays.RemoveAt(0);
                    map.Refresh();
                }
            }
        }

        private void primOlmadıgıIcinYapiyorum()
        {
            if (konumx != 0 && konumy != 0)
            {
                conn.Open();
                String sorgu = "select kargox,kargoy from kargo";
                NpgsqlCommand comm = new NpgsqlCommand(sorgu, conn);
                NpgsqlDataReader dr = comm.ExecuteReader();


                points.Add(new PointLatLng(konumx, konumy));
                for (int i = 0; dr.Read(); i++)
                {
                    String x, y;
                    double lat, lng;
                    x = dr.GetString(0);
                    y = dr.GetString(1);
                    lat = Convert.ToDouble(x);
                    lng = Convert.ToDouble(y);
                    points.Add(new PointLatLng(lat, lng));
                }

                conn.Close();
                for (int i = 0; i < kargosayisi-1; i++)
                {
                    var route = GoogleMapProvider.Instance.GetRoute(points[i], points[i+1], false, false, 14);
                    var r = new GMapRoute(route.Points, "my");

                    var routes = new GMapOverlay("routes");
                    routes.Routes.Add(r);
                    map.Overlays.Add(routes);
                }
            }
        }

    }
}
