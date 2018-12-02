namespace DM.Logic
{
    public static class Constants
    {
        public const int DEFAULT_DB_TAKE_VALUE = 10;
        public const int MEAL_SCHEDULE_FETCH_RANGE_IN_DAYS = 7;

        public const string Base64ExtensionRegex = @"data:image/(?<type>.+?);base64,(?<data>.+)";
        public const string Base64Image = @"^data:image\/(?<plainBase64String>\S+)";

        public const int SALT_SIZE = 16;
        public const int HASH_SIZE = 20;
        public const int ITERATIONS_COUNT = 20;
    }
}
