using AutoMapper;
using NSubstitute;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.CategorySpecifications;

namespace TaskHub.Tests.Bll.Services
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _categoryService = new CategoryService(_unitOfWork, _mapper);
        }

        [Test]
        public async Task UpdateCategoriesAsync_WhenCategoriesIsNull_ReturnsNull()
        {
            // Arrange
            ICollection<string>? categories = null;

            // Act
            var result = await _categoryService.UpdateCategoriesAsync(categories);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task UpdateCategoriesAsync_WhenCategoriesIsEmpty_ReturnsNull()
        {
            // Arrange
            var categories = new List<string>();

            // Act
            var result = await _categoryService.UpdateCategoriesAsync(categories);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task UpdateCategoriesAsync_WhenNewCategoriesExist_ReturnsUpdatedCategories()
        {
            // Arrange
            var existingCategories = new List<CategoryEntity>
            {
                new CategoryEntity { Name = "Category1" },
                new CategoryEntity { Name = "Category2" }
            };
            _unitOfWork.CategoryRepository.GetAsync(Arg.Any<GetCategoriesByNamesSpecification>())
                .Returns(existingCategories);
            var newCategories = new List<string> { "Category3", "Category4" };

            // Act
            var result = await _categoryService.UpdateCategoriesAsync(newCategories);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(existingCategories.Count + newCategories.Count);
            foreach (var category in newCategories)
            {
                result.Should().ContainSingle(c => c.Name == category);
            }
        }
    }
}
