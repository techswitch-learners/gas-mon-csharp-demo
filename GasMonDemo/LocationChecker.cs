using System.Collections.Generic;
using System.Linq;

namespace GasMonDemo
{
    public class LocationChecker
    {
        private readonly IEnumerable<string> _validLocationIds;

        public LocationChecker(IEnumerable<Location> validLocation)
        {
            _validLocationIds = validLocation.Select(location => location.Id);
        }

        public bool FromValidLocation(ReadingMessage readingMessage)
        {
            return _validLocationIds.Contains(readingMessage.Reading.LocationId);
        }
    }
}