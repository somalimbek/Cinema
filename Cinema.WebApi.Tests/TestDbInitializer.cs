using Cinema.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cinema.WebApi.Tests
{
    public static class TestDbInitializer
    {
        public static void Initialize(CinemaContext context)
        {

            #region Movies

            var movies = new List<Movie>()
            {
                new Movie
                {
                    Title = "Tűzgyűrű",
                    Director = "Guillermo Del Toro",
                    Cast = "Charlie Hunnam, Idris Elba, Rinko Kikuchi",
                    Storyline = "A Csendes-óceán mélyén megnyílik egy átjáró, amelyen keresztül óriásszörnyek támadnak a Földre, hogy megszerezzék világunkat. Hamar nyilvánvalóvá válik, hogy a háború nem tarthat sokáig, az emberiség napjai meg vannak számlálva. Még a hatalmas robotok - amelyeket több pilóta irányít, akiknek agya neuronhidakkal van összekapcsolva - sem segítenek. A volt pilóta és egy újonc azonban beindítanak egy kiselejtezett robotot, és elindulnak, hogy megállítsák a betolakodókat, és megmentsék a Földet.",
                    Runtime = 131,
                    Added = new DateTime(2013, 7, 11)
                },
                new Movie
                {
                    Title = "A Grand Budapest Hotel",
                    Director = "Wes Anderson",
                    Cast = "Ralph Fiennes, Tony Revolori, Jude Law",
                    Storyline = "Gustave a híres európai szálloda, a Grand Budapest Hotel legendás főportása a két világháború között. Az elegáns szállodában átlagosnak nemigen mondható vendégek fordulnak meg, arisztokraták, vénkisasszonyok és műkincstolvajok. A főportás összebarátkozik az egyszerű londinerrel, Zero Mustafával, és együtt keverednek bele az évszázad műkincsrablásába. Miközben a világ drámai módon kezd megáltozni körülöttük, a szálloda szinte minden vendége és dolgozója részese lesz az értékes kép utáni hajszának.",
                    Runtime = 100,
                    Added = new DateTime(2014, 3, 20)
                },
                new Movie
                {
                    Title = "Csillagok között",
                    Director = "Christopher Nolan",
                    Cast = "Ralph Fiennes, Tony Revolori, Jude Law",
                    Storyline = "A legendásan titkolózó rendező új sci-fijéről annyit már tudni, hogy csupa sztár működik közre benne és a csillagok között játszódik. Tudósok felfedeznek egy féreglyukat az űrben, és egy csapatnyi felfedező meg kalandor nekivág, hogy átlépje mindazokat a határokat, amelyeket addig áthághatatlannak hittünk: túl akarnak lépni téren és időn.",
                    Runtime = 169,
                    Added = new DateTime(2014, 11, 6)
                }
            };

            movies.ForEach(movie => context.Movies.Add(movie));

            #endregion

            #region Screens

            var screens = new List<Screen>()
            {
                new Screen
                {
                    Name = "Törőcsik",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                },
                new Screen
                {
                    Name = "Bajor",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                },
                new Screen
                {
                    Name = "Korda",
                    NumberOfRows = 15,
                    SeatsPerRow = 30
                }
            };

            screens.ForEach(screen => context.Screens.Add(screen));

            #endregion

            #region Showtimes

            var showtimes = new List<Showtime>();

            for (int i = 0; i < movies.Count; i++)
            {
                showtimes.AddRange(CreateShowtimes(movies[i], screens[i]));
            }

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
                            Status = seatNumber <= 2 ? SeatStatus.Booked : SeatStatus.Free
                        });
                    }
                }
            }

            seats.ForEach(seat => context.Seats.Add(seat));

            #endregion

            context.SaveChanges();
        }

        private static List<Showtime> CreateShowtimes(Movie movie, Screen screen)
        {
            const int openingTime = 15;
            const int numberOfDays = 7;
            const int numberOfShowtimesPerDay = 3;
            double firstShowtime = openingTime;

            var runtime = movie.Runtime;
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
