public class Cliente{
      public int ClienteId { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    
    public Cliente(ViewCliente clien){
        this.ClienteId=clien.ClienteId;
        this.Nombre=clien.Nombre;
        this.Email=clien.Email;
        this.Telefono=clien.Telefono;
    }
      public Cliente()
    {
    }
}