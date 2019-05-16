namespace Wikiled.Instagram.Api.Classes.Models.Location
{
    public class InstaLocation : InstaLocationShort
    {
        public string City { get; set; }

        public long FacebookPlacesId { get; set; }

        public double Height { get; set; }

        public long Pk { get; set; }

        public double Rotation { get; set; }

        public string ShortName { get; set; }

        public double Width { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
    }
}
