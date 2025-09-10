namespace Calendar.Meetings;

public static class CzechHolidaysProvider
{
    public static List<object> Get()
    {
        // Zde by se použil NuGet package jako PublicHoliday nebo Nager.Date
        var holidays = new List<(string name, DateTime date)>
        {
            ("Svátek: Nový rok", new DateTime(DateTime.Now.Year, 1, 1)),
            ("Svátek: Velký pátek", new DateTime(DateTime.Now.Year, 4, 7)),
            ("Svátek: Velikonoční pondělí", new DateTime(DateTime.Now.Year, 4, 10)),
            ("Svátek: Svátek práce", new DateTime(DateTime.Now.Year, 5, 1)),
            ("Svátek: Den vítězství", new DateTime(DateTime.Now.Year, 5, 8)),
            ("Svátek: Den slovanských věrozvěstů", new DateTime(DateTime.Now.Year, 7, 5)),
            ("Svátek: Den upálení mistra Jana Husa", new DateTime(DateTime.Now.Year, 7, 6)),
            ("Svátek: Den české státnosti", new DateTime(DateTime.Now.Year, 9, 28)),
            ("Svátek: Den vzniku samostatného československého státu", new DateTime(DateTime.Now.Year, 10, 28)),
            ("Svátek: Den boje za svobodu a demokracii", new DateTime(DateTime.Now.Year, 11, 17)),
            ("Svátek: Štědrý den", new DateTime(DateTime.Now.Year, 12, 24)),
            ("Svátek: 1. svátek vánoční", new DateTime(DateTime.Now.Year, 12, 25)),
            ("Svátek: 2. svátek vánoční", new DateTime(DateTime.Now.Year, 12, 26))
        };

        return holidays.Select(h => new
        {
            title = h.name,
            start = h.date.ToString("yyyy-MM-dd"),
            display = "background",
            color = "#ffebee"
        }).ToList<object>();
    }
}