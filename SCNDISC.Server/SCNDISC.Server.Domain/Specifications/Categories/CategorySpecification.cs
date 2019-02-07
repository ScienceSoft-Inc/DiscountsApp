using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Exceptions.Categories;

namespace SCNDISC.Server.Domain.Specifications.Categories
{
    public class CategorySpecification : ISpecification<Category>
    {
        public bool IsSatisfiedBy(Category subject)
        {
            if (subject == null)
            {
                throw new InvalidCategoryException("category must not be null");
            }

            if (subject.IsDeleted)
            {
                throw new InvalidCategoryException("Cannot modify deleted branch");
            }

            return true;
        }
    }
}
