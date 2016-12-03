namespace GetWelds.ViewModels
{
   public class Tool
    {
            
       public int Number { get; set; }
       public double X { get; set; }
       public double Y { get; set; }
       public double Z { get; set; }
       public double A { get; set; }
       public double B { get; set; }
       public double C { get; set; }


       public bool IsEmpty
       {
           get { return X == 0 && X == 0 && Z == 0 && A == 0 && B == 0 && C == 0; }
       }

    }
}
