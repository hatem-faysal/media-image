# media-image
package media upload image asp net core 9
---------------------------------------------------------------------

1-upload script to make table media info\DB\script.sql
2-make new model media  Models\Media.cs
3-class all use media Services\ImageServices.cs
4-add new model use function 
        [NotMapped]
        public IFormFile ?Image { get; set; }
        
5-connection media in  Data\DataDbContext.cs
       builder.Entity<Media>().ToTable("media");
       public DbSet<Media> Medias { get; set; }
6-make controller use media 
    implement  Constrict
    
        private readonly DataDbContext _context;
        private readonly IWebHostEnvironment _webHost;
        public ProductController(DataDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }    
        
Create store
    await ImageServices.StoreImage(product, product.Image, _webHost, _context);
Edit store
    if (product.Image != null)
    {
        await ImageServices.EditImage(existingProduct, product.Image, _webHost, _context);
    }
Delete store    
    await ImageServices.DeleteImage(product, _webHost, _context);
    
7-show All data index 
     src="@ImageServices.GetImageUrl(item, (IWebHostEnvironment)Context.RequestServices.GetService(typeof(IWebHostEnvironment)))" 
8-show data edit
     src="@ImageServices.GetImageUrl(Model, (IWebHostEnvironment)Context.RequestServices.GetService(typeof(IWebHostEnvironment)))"

