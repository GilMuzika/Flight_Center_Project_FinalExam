using System;

namespace DataBase_Generator_WPF
{

   public class Name
   {
   public String title { get; set; }
   public String first { get; set; }
   public String last { get; set; }
   }

   public class Street
   {
   public int number { get; set; }
   public String name { get; set; }
   }

   public class Coordinates
   {
   public String latitude { get; set; }
   public String longitude { get; set; }
   }

   public class Timezone
   {
   public String offset { get; set; }
   public String description { get; set; }
   }

   public class Location
   {
    public Street street { get; set; }
   public String city { get; set; }
   public String state { get; set; }
   public String country { get; set; }
   public string postcode { get; set; }
    public Coordinates coordinates { get; set; }
    public Timezone timezone { get; set; }
   }

   public class Login
   {
   public String uuid { get; set; }
   public String username { get; set; }
   public String password { get; set; }
   public String salt { get; set; }
   public String md5 { get; set; }
   public String sha1 { get; set; }
   public String sha256 { get; set; }
   }

   public class Dob
   {
   public DateTime date { get; set; }
   public int age { get; set; }
   }

   public class Registered
   {
   public DateTime date { get; set; }
   public int age { get; set; }
   }

   public class Id
   {
   public String name { get; set; }
   public String value { get; set; }
   }

   public class Picture
   {
   public String large { get; set; }
   public String medium { get; set; }
   public String thumbnail { get; set; }
   }

   public class Results
   {
   public String gender { get; set; }
    public Name name { get; set; }
    public Location location { get; set; }
   public String email { get; set; }
    public Login login { get; set; }
    public Dob dob { get; set; }
    public Registered registered { get; set; }
   public String phone { get; set; }
   public String cell { get; set; }
    public Id id { get; set; }
    public Picture picture { get; set; }
   public String nat { get; set; }
   }

   public class Info
   {
   public String seed { get; set; }
   public int results { get; set; }
   public int page { get; set; }
   public String version { get; set; }
   }

   public class RandomUserAPIObject
   {
    public Results[] results { get; set; }
    public Info info { get; set; }
   }

}

