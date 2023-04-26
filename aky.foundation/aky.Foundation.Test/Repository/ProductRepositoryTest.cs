namespace Diatly.Foundation.Test.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Force.DeepCloner;
    using global::Diatly.Foundation.Test.Domain;
    using global::Diatly.Foundation.Test.Repository.Interfaces;
    using Xunit;

    public partial class ProductRepositoryTest : BaseFixture
    {
        private IProductRepository productRepository;

        public ProductRepositoryTest()
        {
            this.productRepository = this.ProductRepository;
        }

        [Theory]
        [MemberData(nameof(SingleProduct))]
        public async Task ProductRepositoryTest_AddAsync_ReturnTrue(Product product)
        {
            Product pid = product.DeepClone();

            var entity = await this.ProductRepository.AddAsync(pid);

            Assert.NotNull(entity);
            Assert.True(entity.Id != 0);
        }

        [Theory]
        [MemberData(nameof(Products))]
        public async Task ProductRepositoryTest_AddRange_ReturnTrue(params Product[] products)
        {
            Product[] productsToAdd = products.DeepClone();

            var createdProducts = await this.ProductRepository.AddRangeAsync(productsToAdd);

            foreach (var product in createdProducts)
            {
                Assert.NotNull(product);
                Assert.True(product.Id != 0);
            }
        }

        [Theory]
        [MemberData(nameof(SingleProduct))]
        public async Task ProductRepositoryTest_UpdateAsync_ReturnTrue(Product product)
        {
            Product pid = product.DeepClone();

            var entity = await this.ProductRepository.AddAsync(pid);

            entity.AvailableStock = 10;

            var updatedProduct = await this.ProductRepository.UpdateAsync(entity, entity.Id);

            Assert.True(entity.AvailableStock == 10);
        }

        [Theory]
        [MemberData(nameof(SingleProduct))]
        public async Task ProductRepositoryTest_DeleteAsync_ReturnTrue(Product product)
        {
            Product pid = product.DeepClone();

            var entity = await this.ProductRepository.AddAsync(pid);

            int result = await this.ProductRepository.DeleteAsync(entity);

            Assert.True(result != 0);
        }

        [Theory]
        [MemberData(nameof(SingleProduct))]
        public async Task ProductRepositoryTest_GetAsync_ReturnTrue(Product product)
        {
            Product pid = product.DeepClone();

            var entity = await this.ProductRepository.AddAsync(pid);

            var newProduct = await this.ProductRepository.GetAsync(entity.Id);

            Assert.NotNull(newProduct);
        }

        [Theory]
        [MemberData(nameof(SingleProduct))]
        public async Task ProductRepositoryTest_FindAsync_ReturnTrue(Product product)
        {
            Product pid = product.DeepClone();

            var entity = await this.ProductRepository.AddAsync(pid);

            var newProduct = await this.ProductRepository.FindAsync(a => a.Id == entity.Id);

            Assert.NotNull(newProduct);
        }

        [Theory]
        [MemberData(nameof(Products))]
        public async Task ProductRepositoryTest_FindAllAsync_ReturnTrue(params Product[] products)
        {
            Product[] productsToAdd = products.DeepClone();

            var createdProducts = await this.ProductRepository.AddRangeAsync(productsToAdd);

            var findResult = await this.ProductRepository.FindAllAsync(a => a.BrandID == 1701);

            Assert.True(findResult.Count > 0);
        }

        public static IEnumerable<object[]> SingleProduct
        {
            get
            {
                yield return new Product[]
                {
                new Product()
                {
                    ProductName = "This is test product",
                    Description = "Étagère avec structure en bois d'acacia massif et plateaux en ciment. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 22800.00M,
                },
                };
            }
        }

        public static IEnumerable<object[]> Products()
        {
            yield return new Product[]
            {
                new Product()
                {
                    ProductName = "Étagère Merida, 67cm",
                    Description = "Étagère avec structure en bois d'acacia massif et plateaux en ciment. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 22800.00M,
                },
                new Product()
                {
                    ProductName = "Étagère Merida, 97cm",
                    Description = "Étagère avec structure en bois d'acacia massif et plateaux en ciment. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 34680.00M,
                },
                new Product()
                {
                    ProductName = "Set de 2 vases Marta, prisme",
                    Description = "Set de 2 vases en ciment. Convenant pour usage intérieur et extérieur.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 5880.00M,
                },
                new Product()
                {
                    ProductName = "Set de 2 vases Marta, cylindre",
                    Description = "Set de 2 vases en ciment. Convenant pour usage intérieur et extérieur.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 4560.00M,
                },
                new Product()
                {
                    ProductName = "Set de 2 coupes Marta, gris et marron",
                    Description = "Set de 2 coupes en ciment. Convenant pour usage intérieur et extérieur.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 4320.00M,
                },
                new Product()
                {
                    ProductName = "Chaise Brianne, gris foncé",
                    Description = "Chaise avec structure en acier galvanisé peint par pulvérisation. Siège et dossier en corde de polyester résistant aux rayons UV. Empilable. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 14640.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Taos",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 172,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Alisha",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 182,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Ariane",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 140,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Auburn",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 193,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Hopi",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 181,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Williams",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 180,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Sandie",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 180,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Flamingo",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 147,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Set de 2 coupes Marta, beige et gris",
                    Description = "Set de 2 coupes en ciment. Convenant pour usage intérieur et extérieur.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 3480.00M,
                },
                new Product()
                {
                    ProductName = "Set de 2 coupes Marta, beige et marron",
                    Description = "Set de 2 coupes en ciment. Convenant pour usage intérieur et extérieur.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 3840.00M,
                },
                new Product()
                {
                    ProductName = "Set de 2 plateaux Marta",
                    Description = "Set de 2 plateaux en ciment. Convenant pour usage intérieur et extérieur.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 6960.00M,
                },
                new Product()
                {
                    ProductName = "Pouf Taos",
                    Description = "Pouf en tissu 100% acrylique, déhoussable. Rempli avec polystyrène expansé. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 7440.00M,
                },
                new Product()
                {
                    ProductName = "Pouf Ariane",
                    Description = "Pouf en tissu 100% acrylique, déhoussable. Rempli avec polystyrène expansé. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 7440.00M,
                },
                new Product()
                {
                    ProductName = "Pouf Alisha",
                    Description = "Pouf en tissu 100% acrylique, déhoussable. Rempli avec polystyrène expansé. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 7440.00M,
                },
                new Product()
                {
                    ProductName = "Pouf Williams",
                    Description = "Pouf en tissu 100% acrylique, déhoussable. Rempli avec polystyrène expansé. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 7440.00M,
                },
                new Product()
                {
                    ProductName = "Pouf Ashleen",
                    Description = "Pouf en tissu 100% acrylique, déhoussable. Rempli avec polystyrène expansé. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 5760.00M,
                },
                new Product()
                {
                    ProductName = "Pouf Ashleen",
                    Description = "Pouf en tissu 100% acrylique, déhoussable. Rempli avec polystyrène expansé. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 5760.00M,
                },
                new Product()
                {
                    ProductName = "Pouf Anneke",
                    Description = "Pouf en tissu 100% acrylique, déhoussable. Rempli avec polystyrène expansé. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 5640.00M,
                },
                new Product()
                {
                    ProductName = "Coussin Valle",
                    Description = "Coussin en tissu 100% polyester, déhoussable. Remplissage de fibre inclus. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 177,
                    BrandID = 1701,
                    Price = 1680.00M,
                },
                new Product()
                {
                    ProductName = "Table pliante Merida",
                    Description = "Table d'appoint avec structure en bois d'acacia massif et plateau amovible en ciment. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 7680.00M,
                },
                new Product()
                {
                    ProductName = "Set de 2 tables Aurelie",
                    Description = "Set de 2 tables d'appoint de ciment avec pieds en métal. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 24600.00M,
                },
                new Product()
                {
                    ProductName = "Table d'appoint Norah, beige Ø40cm",
                    Description = "Table d'appoint avec structure en acier galvanisé peint par pulvérisation recouverte avec corde de polyester résistant aux rayons UV. Plateau en poly-cement Superstone. Pour usage intérieur et extérieur à l'abri ou couvert.",
                    Category = new Category() { Name = "Test category" },
                    AvailableStock = 0,
                    BrandID = 1701,
                    Price = 16440.00M,
                },
                new Product()
                {
                    ProductName = "Dormeuses navette Mekong",
                    Description = "<p> \tFinition or 24 carats.</p> <p> \tLes bijoux Dear Charlotte sont dor&eacute;s &agrave; l&#39;or fin puis sabl&eacute;s pour donner cet aspect noble et sensuel.</p> <p> \tToutes nos collections sont inspir&eacute;es de l&#39;univers romantique propre &agrave; la r&ecirc;verie et &agrave; la promesse.</p>",
                    Category = new Category() { Name = "Category 349" },
                    AvailableStock = 20,
                    BrandID = 227,
                    Price = 70.00M,
                },
                new Product()
                {
                    ProductName = "Bague navette Mekong",
                    Description = "<p> \tR&eacute;glable</p> <p> \tDor&eacute; finition or 24 carats.</p>",
                    Category = new Category() { Name = "Category 348" },
                    AvailableStock = 20,
                    BrandID = 227,
                    Price = 60.00M,
                },
                new Product()
                {
                    ProductName = "Bague fleur de lys et amazonite Mekong",
                    Description = "<p> \tR&eacute;glable</p> <p> \tDor&eacute; finition or 24 carats.</p>",
                    Category = new Category() { Name = "Category 348" },
                    AvailableStock = 20,
                    BrandID = 227,
                    Price = 80.00M,
                },
                new Product()
                {
                    ProductName = "Nappe coton carreaux rouge Nelly",
                    Description = "<p> \tAux amis de la tradition, des rec&eacute;ptions, que vous soyez particulier ou professionnel de la restauration, ce tissu doit vous inspirer!</p> <p> \tConfectionn&eacute; dans l&#39;atelier Clara Linge au coeur des Vosges, vous ajouterez une touche de convivialit&eacute; &agrave; vos repas de famille.</p> <p> \tPropos&eacute;e ici du format 150x150 &agrave; 150x400, nous pouvons &eacute;galement les confectionner sans limite de taille.&nbsp;</p> <p> \tVous pouvez &eacute;galement l&#39;assortir aux serviettes de table dans le m&ecirc;me tissu.</p> <p> \tColoris grands teints. Lavage en machine &agrave; 60&deg;C recommand&eacute; (90&deg;C support&eacute; occasionnellement)</p> <p> \tFabriqu&eacute; en france.</p>",
                    Category = new Category() { Name = "Category 1654" },
                    AvailableStock = 8,
                    BrandID = 1444,
                    Price = 29.50M,
                },
                new Product()
                {
                    ProductName = "Sac cabas en cuir de Veau pleine Fleur",
                    Description = "<p> \tLigne Aurore propose un style casual, minimal, affirm&eacute; et moderne. Des accessoires en s&eacute;rie limit&eacute;e, sacs et pochettes haut de gamme. Travail du cuir brut ou m&eacute;langes de mati&egrave;res.&nbsp;</p> <p> \tLe sac Bony est un sac f&eacute;minin, simple et &eacute;l&eacute;gant. L&#39;originalit&eacute; de sa coupe &agrave; brut, lui donne un style minimal. R&eacute;alis&eacute; en cuir de Veau pleine fleur Fuschia. Ce petit format permet de ranger toutes vos affaires personnelles. Anse plate. Fermeture bouton-pression grav&eacute;.&nbsp;</p> <p> \tDimensions : 40cm - 28cm - 12cm</p> <p> \tMati&egrave;res : Cuir de Vachette Italien de qualit&eacute;.&nbsp;</p> <br>",
                    Category = new Category() { Name = "Category 458" },
                    AvailableStock = 1,
                    BrandID = 485,
                    Price = 168.00M,
                },
                new Product()
                {
                    ProductName = "Sac seau en cuir Giselle",
                    Description = "<p> \tL27cm X H35cm X P14cm.</p> <p> \tCuir de Veau Italien - coloris Terracotta - Int&eacute;rieur brut en cuir - Anse r&eacute;glable - logo grav&eacute;</p> <p> \tFabriqu&eacute; en France.</p>",
                    Category = new Category() { Name = "Category 464" },
                    AvailableStock = 1,
                    BrandID = 485,
                    Price = 175.00M,
                },
                new Product()
                {
                    ProductName = "Sac seau en cuir Giselle",
                    Description = "<p> \tL27cm X H35cm X P14cm.</p> <p> \tCuir de Veau Italien - coloris Terracotta - Int&eacute;rieur brut en cuir - Anse r&eacute;glable - logo grav&eacute;</p> <p> \tFabriqu&eacute; en France.</p>",
                    Category = new Category() { Name = "Category 464" },
                    AvailableStock = 1,
                    BrandID = 485,
                    Price = 175.00M,
                },
                new Product()
                {
                    ProductName = "Jonc émaillé turquoise Mékong",
                    Description = "<p> \tS&#39;ajuste facilement sur le poignet.</p> <p> \tFinition or 24 carats.</p> <p> \tLes bijoux Dear Charlotte sont dor&eacute;s &agrave; l&#39;or fin puis sabl&eacute;s pour donner cet aspect noble et sensuel.</p> <p> \tToutes nos collections sont inspir&eacute;es de l&#39;univers romantique propre &agrave; la r&ecirc;verie et &agrave; la promesse.</p>",
                    Category = new Category() { Name = "Category 350" },
                    AvailableStock = 20,
                    BrandID = 227,
                    Price = 100.00M,
                },
                new Product()
                {
                    ProductName = "Etui à lunette",
                    Description = "<p> \tL8cm X H16cm</p> <p> \t&nbsp;</p> <p> \tEtui &agrave; lunette en cuir qui vous ressemble -&nbsp;esprit minimaliste et mixte&nbsp;</p> <p> \t&nbsp;</p> <p> \tExiste multiple coloris</p> <p> \t&nbsp;</p> <p> \tFabriqu&eacute; en France</p>",
                    Category = new Category() { Name = "Category 1634" },
                    AvailableStock = 10,
                    BrandID = 485,
                    Price = 35.00M,
                },
                new Product()
                {
                    ProductName = "Top cosima",
                    Description = "Top en coton bleu ciel imprimé. Manches courtes, grand décolleté V, nouée à l'avant à la taille.<br /> Cet imprimé existe également dans 2 robes : ANANE et ALINDA.<br /><br /> Adeline mesure 1m78 et porte une taille S. Prenez votre taille habituelle.<br /><br /> Composition: 75% Viscose, 25% Polyamide",
                    Category = new Category() { Name = "Category 516" },
                    AvailableStock = 6,
                    BrandID = 1597,
                    Price = 55.00M,
                },
                new Product()
                {
                    ProductName = "Robe alinda",
                    Description = "Robe mi longue dos nu en coton bleu imprimé. Resserrée à la taille, boutonnée dans le dos, jeu de bretelles dans le dos. Les bretelles sont ajustables.<br /> Cet imprimé existe également en robe ANANE et en top COSIMA.<br /><br /> Adeline mesure 1m78 et porte une taille S. Prenez votre taille habituelle.<br /><br /> Composition: 75% Viscose, 25% Polyamide",
                    Category = new Category() { Name = "Category 508" },
                    AvailableStock = 6,
                    BrandID = 1597,
                    Price = 85.00M,
                },
                new Product()
                {
                    ProductName = "Robe anane",
                    Description = "Robe courte en coton bleu ciel imprimé. Coupe évasée, taille élastique, croisée devant et fermée par des boutons dans le dos.<br /> Cet imprimé existe également en robe ALINDA et en top COSIMA.<br /><br /> Adeline mesure 1m78 et porte une taille S/M. Prenez votre taille habituelle.<br /><br /> Composition: 75% Viscose, 25% Polyamide",
                    Category = new Category() { Name = "Category 508" },
                    AvailableStock = 5,
                    BrandID = 1597,
                    Price = 85.00M,
                },
                new Product()
                {
                    ProductName = "Chemise candie",
                    Description = "Chemise bleu à rayures blanches, col chemise, manches basses courtes, nouée à la taille.<br /> La blouse CANDIE est également disponible en robe AURIANE.<br /><br /> Adeline mesure 1m78 et porte une taille S/M. Prenez votre taille habituelle.<br /><br /> Composition: 100% Coton",
                    Category = new Category() { Name = "Category 479" },
                    AvailableStock = 6,
                    BrandID = 1597,
                    Price = 59.00M,
                },
                new Product()
                {
                    ProductName = "Robe auriane",
                    Description = "Robe courte en coton bleu à fines rayures blanches. Coupe évasée, taille élastique, croisée devant et fermée par des boutons dans le dos.<br /> La robe AURIANE est disponible également en top CANDIE.<br /><br /> Adeline mesure 1m78 et porte une taille S/M. Prenez votre taille habituelle.<br /><br /> Composition: 100% Coton",
                    Category = new Category() { Name = "Category 508" },
                    AvailableStock = 4,
                    BrandID = 1597,
                    Price = 79.00M,
                },
                new Product()
                {
                    ProductName = "Pantalon perla",
                    Description = "Pantalon en seersucker rayé rose et blanc. Coupe droite à pinces, poches italiennes. Il s'assorti avec la veste LEONIE pour un total look preppy. <br /> Retrouvez également ces mêmes rayures dans le top CASTILLE ainsi que le short DORIANE.<br /><br /> Adeline mesure 1m78 et porte une taille S. Prenez votre taille habituelle.<br /><br /> Composition: 69% Polyester, 26% Viscose, 5% Elasthanne",
                    Category = new Category() { Name = "Category 494" },
                    AvailableStock = 5,
                    BrandID = 1597,
                    Price = 65.00M,
                },
                new Product()
                {
                    ProductName = "Top chaima",
                    Description = "Top manches courtes dans un tissus très fluide et leger. Manches volantées, grand décolleté V devant et dos avec un lien noué dans le dos.<br /><br /> Adeline mesure 1m78 et porte une taille S. Prenez votre taille habituelle.<br /><br /> Composition: 81.9% Viscose, 18.1% Polyamide",
                    Category = new Category() { Name = "Category 516" },
                    AvailableStock = 12,
                    BrandID = 1597,
                    Price = 55.00M,
                },
                new Product()
                {
                    ProductName = "Top chaima",
                    Description = "Top manches courtes dans un tissus très fluide et leger. Manches volantées, grand décolleté V devant et dos avec un lien noué dans le dos.<br /><br /> Adeline mesure 1m78 et porte une taille S. Prenez votre taille habituelle.<br /><br /> Composition: 81.9% Viscose, 18.1% Polyamide",
                    Category = new Category() { Name = "Category 516" },
                    AvailableStock = 12,
                    BrandID = 1597,
                    Price = 55.00M,
                },
                new Product()
                {
                    ProductName = "Top cassie",
                    Description = "Top en coton noir imprimé à fleurs roses. Épaules dénudées, manches 3/4 aux poignets fermés par des nœuds. Cet imprimé existe également en robe AELLE.<br /><br /> Adeline mesure 1m78 et porte une taille S. Prenez votre taille habituelle.<br /><br /> Composition: 81.7% Viscose, 18.3% Polyamide",
                    Category = new Category() { Name = "Category 516" },
                    AvailableStock = 6,
                    BrandID = 1597,
                    Price = 59.00M,
                },
                new Product()
                {
                    ProductName = "Robe aelle",
                    Description = "Robe mi-longue en coton noir à motif rose fleuri. Manches courtes, décolleté V, nouée à la taille et boutonnée sur tout la longueur. Elle peut se porter décolleté devant ou dos.<br /> Cet imprimé existe également en top CASSIE.<br /><br /> Adeline mesure 1m78 et porte une taille S. Prenez votre taille habituelle.<br /><br /> Composition: 81.7% Viscose, 18.3% Polyamide",
                    Category = new Category() { Name = "Category 508" },
                    AvailableStock = 6,
                    BrandID = 1597,
                    Price = 85.00M,
                },
                new Product()
                {
                    ProductName = "Sac Cabas en cuir de Veau",
                    Description = "<p> \tL40cm X H34cm X P12cm Cuir de Veau Italien - Int&eacute;rieur brut en cuir - Poche ext&eacute;rieure - Anse non r&eacute;glable - Fermeture pression laiton - logo grav&eacute;</p> <p> \tFabriqu&eacute; en France.</p> <p> \t&nbsp;</p> <br>",
                    Category = new Category() { Name = "Category 558" },
                    AvailableStock = 2,
                    BrandID = 485,
                    Price = 188.00M,
                },
                new Product()
                {
                    ProductName = "Grand Sac en cuir Le Ulysse",
                    Description = "<p> \tSac &agrave; main en cuir souple, fabriqu&eacute; en Italie. Tr&egrave;s pratique avec son grand volume, son double compartiment et ses multiples poches zipp&eacute;es (3 ext&eacute;rieures et 2 int&eacute;rieures). Il dispose d&#39;une bandouli&egrave;re r&eacute;glable et amovible et mesure 41 x 31 x 18 cm.</p>",
                    Category = new Category() { Name = "Category 1928" },
                    AvailableStock = 20,
                    BrandID = 1166,
                    Price = 165.00M,
                },
                new Product()
                {
                    ProductName = "Grand Sac en cuir Le Ulysse",
                    Description = "<p> \tSac &agrave; main en cuir souple, fabriqu&eacute; en Italie. Tr&egrave;s pratique avec son grand volume, son double compartiment et ses multiples poches zipp&eacute;es (3 ext&eacute;rieures et 2 int&eacute;rieures). Il dispose d&#39;une bandouli&egrave;re r&eacute;glable et amovible et mesure 41 x 31 x 18 cm.</p>",
                    Category = new Category() { Name = "Category 1928" },
                    AvailableStock = 20,
                    BrandID = 1166,
                    Price = 165.00M,
                },
            };
        }
    }
}
