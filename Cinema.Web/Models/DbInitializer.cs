using Microsoft.AspNetCore.Identity;
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
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //context.Database.EnsureCreated();
            context.Database.Migrate();

            if (context.Movies.Any())
            {
                return;
            }

            userManager.CreateAsync(new ApplicationUser{ UserName = "admin" }, "admin");
            userManager.CreateAsync(new ApplicationUser{ UserName = "soma" }, "soma");
            userManager.CreateAsync(new ApplicationUser{ UserName = "asdf" }, "asdf");

            #region Movies

            var theGrandBudapestPosterHun = Path.Combine(imageDirectory, "agrandbudapesthotel.jpg");
            var kingArthurPosterHun = Path.Combine(imageDirectory, "arthurkiraly.jpg");
            var interstellarPosterHun = Path.Combine(imageDirectory, "csillagokkozott.jpg");
            var johnWickPosterHun = Path.Combine(imageDirectory, "johnwick.jpg");
            var johnWick2PosterHun = Path.Combine(imageDirectory, "johnwick.jpg");
            var isleOfDogsPosterHun = Path.Combine(imageDirectory, "kutyakszigete.jpg");
            var pacificRimPosterHun = Path.Combine(imageDirectory, "tuzgyuru.jpg");
            var theGentlemenPosterHun = Path.Combine(imageDirectory, "uriemberek.jpg");
            var greenBookPosterHun = Path.Combine(imageDirectory, "zoldkonyv.jpg");

            var movies = new List<Movie>()
            {
                new Movie
                {
                    Title = "Tűzgyűrű",
                    Director = "Guillermo Del Toro",
                    Cast = "Charlie Hunnam, Idris Elba, Rinko Kikuchi",
                    Storyline = "A Csendes-óceán mélyén megnyílik egy átjáró, amelyen keresztül óriásszörnyek támadnak a Földre, hogy megszerezzék világunkat. Hamar nyilvánvalóvá válik, hogy a háború nem tarthat sokáig, az emberiség napjai meg vannak számlálva. Még a hatalmas robotok - amelyeket több pilóta irányít, akiknek agya neuronhidakkal van összekapcsolva - sem segítenek. A volt pilóta és egy újonc azonban beindítanak egy kiselejtezett robotot, és elindulnak, hogy megállítsák a betolakodókat, és megmentsék a Földet.",
                    Runtime = 131,
                    Poster = File.Exists(pacificRimPosterHun) ? File.ReadAllBytes(pacificRimPosterHun) : null,
                    Added = new DateTime(2013, 7, 11)
                },
                new Movie
                {
                    Title = "A Grand Budapest Hotel",
                    Director = "Wes Anderson",
                    Cast = "Ralph Fiennes, Tony Revolori, Jude Law",
                    Storyline = "Gustave a híres európai szálloda, a Grand Budapest Hotel legendás főportása a két világháború között. Az elegáns szállodában átlagosnak nemigen mondható vendégek fordulnak meg, arisztokraták, vénkisasszonyok és műkincstolvajok. A főportás összebarátkozik az egyszerű londinerrel, Zero Mustafával, és együtt keverednek bele az évszázad műkincsrablásába. Miközben a világ drámai módon kezd megáltozni körülöttük, a szálloda szinte minden vendége és dolgozója részese lesz az értékes kép utáni hajszának.",
                    Runtime = 100,
                    Poster = File.Exists(theGrandBudapestPosterHun) ? File.ReadAllBytes(theGrandBudapestPosterHun) : null,
                    Added = new DateTime(2014, 3, 20)
                },
                new Movie
                {
                    Title = "Csillagok között",
                    Director = "Christopher Nolan",
                    Cast = "Ralph Fiennes, Tony Revolori, Jude Law",
                    Storyline = "A legendásan titkolózó rendező új sci-fijéről annyit már tudni, hogy csupa sztár működik közre benne és a csillagok között játszódik. Tudósok felfedeznek egy féreglyukat az űrben, és egy csapatnyi felfedező meg kalandor nekivág, hogy átlépje mindazokat a határokat, amelyeket addig áthághatatlannak hittünk: túl akarnak lépni téren és időn.",
                    Runtime = 169,
                    Poster = File.Exists(interstellarPosterHun) ? File.ReadAllBytes(interstellarPosterHun) : null,
                    Added = new DateTime(2014, 11, 6)
                },
                new Movie
                {
                    Title = "John Wick",
                    Director = "Chad Stahelski",
                    Cast = "Keanu Reeves, Michael Nyqvist, Alfie Allen",
                    Storyline = "John Wick (Keanu Reeves) nyugodt életre vágyik. Magányosan akarja tölteni a napjait: kutyája, sportkocsija, üres, hideg lakása éppen elég neki - nincs szüksége többre. De egy nyugdíjas bérgyilkos nem pihenhet. És amikor bántják, ő sem marad tétlen. Előveszi rég elrejtett fegyvereit, és elindul véres bosszúhadjáratára. Egyetlen ember harcol gengszterek és bérgyilkosok egész hadserege ellen, New York pedig valódi csatatérré válik. És az őrült, véres ütközetben mégsem egyértelmű, ki fog győzni: a gyilkosok légiója vagy a magányos harcos. Hiszen ő John Wick.",
                    Runtime = 96,
                    Poster = File.Exists(johnWickPosterHun) ? File.ReadAllBytes(johnWickPosterHun) : null,
                    Added = new DateTime(2014, 11, 13)
                },
                new Movie
                {
                    Title = "John Wick: 2. felvonás",
                    Director = "Basil Iwanyk",
                    Cast = "Keanu Reeves, Riccardo Scamarcio, Ian McShane",
                    Storyline = "John Wick (Keanu Reeves), a visszavonult bérgyilkos sosem nyugszik. Miután a magányos harcos New Yorkot totális csatatérré változtatta, és bosszúhadjárata során egy sereg gyilkost iktatott ki, újra ringbe száll. Wick egykori társa, egy titokzatos nemzetközi bérgyilkos szervezet irányítását akarja megkaparintani magának, így ráveszi cimboráját, hogy ismét csatasorba álljon. Wicket kötelezi az eskü, így vonakodva, de Rómába utazik, ahol a világ legveszélyesebb gyilkosaival kell szembenéznie. És a legenda egy még durvább, még brutálisabb háborúban találja magát.",
                    Runtime = 122,
                    Poster = File.Exists(johnWick2PosterHun) ? File.ReadAllBytes(johnWick2PosterHun) : null,
                    Added = new DateTime(2017, 2, 23)
                },
                new Movie
                {
                    Title = "Arthur király - A kard legendája",
                    Director = "Guy Ritchie",
                    Cast = "Charlie Hunnam, Jude Law",
                    Storyline = "London mindig is London volt. A kora középkor Londoniumában is olyan fazonok, gengszterek, balekok, maffiózók és áldozatok rótták szűk és zsúfolt utcáit, mint manapság. A fiatal Arthur (Charlie Hunnam) sem más: ő és bandája a sötét mellékutcákban dolgozik, és nem ijed meg a saját árnyékától. Az ifjú és ambíciózus bandavezér nem is sejti, hogy királyi vér folyik az ereiben, de amikor egy sziklába döfött kardra talál, megpróbálja magával vinni. És sikerül neki. Az Excalibur ereje alaposabban megváltoztatja az életét, mint eddig az összes környékbeli rendőrfelügyelő. Tagja lesz egy földalatti ellenálló csoportnak, és egy titokzatos bombázó, Guinevere (Astrid Berges-Frisbey) irányítása mellett fokozatosan megérti, milyen varázslat rejtőzik új fegyverében. A kard nélkül semmi esélye, hogy egyesítse népét, és legyőzze a trónbitorló zsarnokot, Vortigernt (Jude Law).",
                    Runtime = 126,
                    Poster = File.Exists(kingArthurPosterHun) ? File.ReadAllBytes(kingArthurPosterHun) : null,
                    Added = new DateTime(2017, 5, 11)
                },
                new Movie
                {
                    Title = "Kutyák szigete",
                    Director = "Guillermo Del Toro",
                    Cast = "Bryan Cranston, Edward Norton, Bill Murray",
                    Storyline = "A KUTYÁK SZIGETE hőse Atari Kobajasi, a korrupt Kobajasi polgármester 12 éves nevelt fia. Mikor egy rendelet értelmében Megaszaki összes kutyáját egy széles kiterjedésű, hulladéklerakóként használt szigetre száműzik, Atari egymaga indul apró repülőjén a Szemét-szigetre, hogy felkutassa testőrkutyáját. Ott egy falka új barát segítségével belevág nagy kalandjába, ami megváltoztatja az egész város jövőjét.", 
                    Runtime = 101,
                    Poster = File.Exists(isleOfDogsPosterHun) ? File.ReadAllBytes(isleOfDogsPosterHun) : null,
                    Added = new DateTime(2018, 5, 3)
                },
                new Movie
                {
                    Title = "Zöld könyv - Útmutató az élethez",
                    Director = "Peter Farrelly",
                    Cast = "Viggo Mortensen, Mahershala Ali, Linda Cardellini",
                    Storyline = "Tony Lip egyszerű, ugyanakkor jó lelkű fickó, az a típus, akinek a problémamegoldó készsége kimerül az „előbb ütök és csak aztán kérdezek” módszerben. Egy kis mellékes reményében elvállalja, hogy egy afroamerikai zongorista, Don Shirley sofőrje lesz, aki Amerika déli államaiba indul turnézni, oda, ahol a helyiek nem látják szívesen azokat, akiknek más a bőrszíne. Don kifinomult stílusa szöges ellentéte az egykori kidobó ember nyers modorának, ám az út során rájönnek, hogy nem is annyira különbözőek.",
                    Runtime = 130 ,
                    Poster = File.Exists(greenBookPosterHun) ? File.ReadAllBytes(greenBookPosterHun) : null,
                    Added = new DateTime(2019, 2, 21)
                },
                new Movie
                {
                    Title = "Úriemberek",
                    Director = "Guy Ritchie",
                    Cast = "Matthew McConaughey, Charlie Hunnam, Michelle Dockery",
                    Storyline = "Amikor híre megy, hogy az amerikai drogkereskedő, Mickey Pearson (Matthew McConaughey) túl akar adni szuperjövedelmező londoni marihuána birodalmán az egész alvilág felbolydul. Rengeteg lóvé forog kockán, nem is csoda, hogy a gengszterek között kirobban a harc. Bérgyilkosok, kisstílű bűnözők, nagypályások, dílerek, mindenféle kétes alakok bukkannak fel a meggazdagodás reményében. A helyzet kezd igen csak eldurvulni: egyre több hulla potyog, egyre több fegyver kerül terítékre, ám azt mégis nehéz lenne megjósolni, hogy végül kinek a kezében landol a csinos drogbiznisz.",
                    Runtime = 113,
                    Poster = File.Exists(theGentlemenPosterHun) ? File.ReadAllBytes(theGentlemenPosterHun) : null,
                    Added = new DateTime(2020, 1, 30)
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
                    Name = "Vajna",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                },
                new Screen
                {
                    Name = "Kabos",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                },
                new Screen
                {
                    Name = "Latabár",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                },
                new Screen
                {
                    Name = "Radványi",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                },
                new Screen
                {
                    Name = "Jávor",
                    NumberOfRows = 8,
                    SeatsPerRow = 15
                },
                new Screen
                {
                    Name = "Karády",
                    NumberOfRows = 10,
                    SeatsPerRow = 20
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
