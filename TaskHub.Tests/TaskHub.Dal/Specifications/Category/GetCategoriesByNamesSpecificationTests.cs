using TaskHub.Dal.Entities;
using TaskHub.Dal.Specification.CategorySpecifications;

namespace TaskHub.Tests.TaskHub.Dal.Specifications.Category
{
    public class GetCategoriesByNamesSpecificationTests
    {
        [Test]
        public void GetCategoriesByNamesSpecificationTests_ShouldFilterCategoriesByNames()
        {
            // Arrange
            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = Guid.NewGuid(), Name = "Category 1" },
                new CategoryEntity { Id = Guid.NewGuid(), Name = "Category 2" },
                new CategoryEntity { Id = Guid.NewGuid(), Name = "Category 3" }
            };
            var categoryNames = new List<string> { "Category 1", "Category 3" };
            var spec = new GetCategoriesByNamesSpecification(categoryNames);

            // Act
            var results = spec.Evaluate(categories.AsQueryable()).ToList();

            // Assert
            results.Should().HaveCount(2);
            results.Should().Contain(r => r.Id == categories[0].Id);
            results.Should().Contain(r => r.Id == categories[2].Id);
        }
    }
}
