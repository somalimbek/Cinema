using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public static class DbInitializer
    {

        public static void Initialize(IServiceProvider serviceProvider, string imageDirectory)
        {
            CinemaContext context = serviceProvider.GetRequiredService<CinemaContext>();

            //context.Database.EnsureCreated();
            context.Database.Migrate();

            if (context.Movies.Any())
            {
                return;
            }

            #region Movies

            var theGentlemenPosterHun = Path.Combine(imageDirectory, "uriemberek.jpg");
            var kingArthurPosterHun = Path.Combine(imageDirectory, "arthurkiraly.jpg");
            var pacificRimPosterHun = Path.Combine(imageDirectory, "tuzgyuru.jpg");

            var movies = new List<Movie>()
            {
                new Movie
                {
                    Title = "Úriemberek",
                    Director = "Guy Ritchie",
                    Cast = "Matthew McConaughey, Charlie Hunnam, Michelle Dockery",
                    Storyline = "Amikor híre megy, hogy az amerikai drogkereskedő, Mickey Pearson (Matthew McConaughey) túl akar adni szuperjövedelmező londoni marihuána birodalmán az egész alvilág felbolydul. Rengeteg lóvé forog kockán, nem is csoda, hogy a gengszterek között kirobban a harc. Bérgyilkosok, kisstílű bűnözők, nagypályások, dílerek, mindenféle kétes alakok bukkannak fel a meggazdagodás reményében. A helyzet kezd igen csak eldurvulni: egyre több hulla potyog, egyre több fegyver kerül terítékre, ám azt mégis nehéz lenne megjósolni, hogy végül kinek a kezében landol a csinos drogbiznisz.",
                    Runtime = 113,
                    Poster = File.Exists(theGentlemenPosterHun) ? File.ReadAllBytes(theGentlemenPosterHun) : null,
                    Added = DateTime.Now
                },
                new Movie
                {
                    Title = "Arthur király - A kard legendája",
                    Director = "Guy Ritchie",
                    Cast = "Charlie Hunnam, Jude Law",
                    Storyline = "London mindig is London volt. A kora középkor Londoniumában is olyan fazonok, gengszterek, balekok, maffiózók és áldozatok rótták szűk és zsúfolt utcáit, mint manapság. A fiatal Arthur (Charlie Hunnam) sem más: ő és bandája a sötét mellékutcákban dolgozik, és nem ijed meg a saját árnyékától. Az ifjú és ambíciózus bandavezér nem is sejti, hogy királyi vér folyik az ereiben, de amikor egy sziklába döfött kardra talál, megpróbálja magával vinni. És sikerül neki. Az Excalibur ereje alaposabban megváltoztatja az életét, mint eddig az összes környékbeli rendőrfelügyelő. Tagja lesz egy földalatti ellenálló csoportnak, és egy titokzatos bombázó, Guinevere (Astrid Berges-Frisbey) irányítása mellett fokozatosan megérti, milyen varázslat rejtőzik új fegyverében. A kard nélkül semmi esélye, hogy egyesítse népét, és legyőzze a trónbitorló zsarnokot, Vortigernt (Jude Law).",
                    Runtime = 126,
                    Poster = File.Exists(kingArthurPosterHun) ? File.ReadAllBytes(kingArthurPosterHun) : null,
                    Added = DateTime.Now
                },
                new Movie
                {
                    Title = "Tűzgyűrű",
                    Director = "Guillermo Del Toro",
                    Cast = "Charlie Hunnam, Idris Elba, Rinko Kikuchi",
                    Storyline = "A Csendes-óceán mélyén megnyílik egy átjáró, amelyen keresztül óriásszörnyek támadnak a Földre, hogy megszerezzék világunkat. Hamar nyilvánvalóvá válik, hogy a háború nem tarthat sokáig, az emberiség napjai meg vannak számlálva. Még a hatalmas robotok - amelyeket több pilóta irányít, akiknek agya neuronhidakkal van összekapcsolva - sem segítenek. A volt pilóta és egy újonc azonban beindítanak egy kiselejtezett robotot, és elindulnak, hogy megállítsák a betolakodókat, és megmentsék a Földet.",
                    Runtime = 131,
                    Poster = File.Exists(pacificRimPosterHun) ? File.ReadAllBytes(pacificRimPosterHun) : null,
                    Added = DateTime.Now
                }
            };

            movies.ForEach(movie => context.Movies.Add(movie));

            #endregion

            #region Screens

            var screens = new List<Screen>()
            {
                new Screen
                {
                    Name = "Korda",
                    NumberOfRows = 15,
                    SeatsPerRow = 30
                },
                new Screen
                {
                    Name = "Karády",
                    NumberOfRows = 10,
                    SeatsPerRow = 20
                },
                new Screen
                {
                    Name = "Jávor",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                }
            };

            screens.ForEach(screen => context.Screens.Add(screen));

            #endregion

            #region Showtimes

            var showtimes = new List<Showtime>();

            showtimes.AddRange(CreateShowtimes(movies[0], screens[0], 113, 0.5));
            showtimes.AddRange(CreateShowtimes(movies[1], screens[1], 126, 0.25));
            showtimes.AddRange(CreateShowtimes(movies[2], screens[2], 131, 0));

            showtimes.ForEach(showtime => context.Showtimes.Add(showtime));

            #endregion

            #region Seats

            var seats = new List<Seat>();

            foreach (var showtime in showtimes)
            {
                for (int rowNumber = 1; rowNumber <= showtime.Screen.NumberOfRows; rowNumber++)
                {
                    for (int seatNumber = 1; seatNumber <= showtime.Screen.SeatsPerRow; seatNumber++)
                    {
                        seats.Add(new Seat
                        {
                            Showtime = showtime,
                            RowNumber = rowNumber,
                            SeatNumber = seatNumber,
                            Status = SeatStatus.Free
                        });
                    }
                }
            }

            seats.ForEach(seat => context.Seats.Add(seat));

            #endregion

            #region Employees

            #endregion

            context.SaveChanges();
        }

        private static List<Showtime> CreateShowtimes(Movie movie, Screen screen, int runtime, double hoursTillFirstShow)
        {
            const int openingTime = 16;
            const int numberOfDays = 7;
            const int numberOfShowtimesPerDay = 3;
            double firstShowtime = openingTime + hoursTillFirstShow;
            
            var timeBetweenShowTimes = CalculateBreakBetweenShowtimes(runtime, 15);

            var showtimes = new List<Showtime>();

            for (int day = 0; day < numberOfDays; day++)
            {
                for (int showtimeNumber = 0; showtimeNumber < numberOfShowtimesPerDay; showtimeNumber++)
                {
                    showtimes.Add(new Showtime
                    {
                        Movie = movie,
                        Time = DateTime.Today
                            .AddDays(day)
                            .AddHours(firstShowtime)
                            .AddMinutes(showtimeNumber * timeBetweenShowTimes),
                        Screen = screen
                    });
                }
            }

            return showtimes;
        }

        private static int CalculateBreakBetweenShowtimes(int runTime, int minTimeBetweenShowtimes)
        {
            return Convert.ToInt32(Math.Ceiling((runTime + minTimeBetweenShowtimes) / 10.0) * 10);
        }
    }
}
