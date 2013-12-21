using System.Linq;
using BuilderFramework.Exceptions;
using BuilderFramework.Tests.TestTypes;
using FluentAssertions;
using NUnit.Framework;

namespace BuilderFramework.Tests
{
    [TestFixture]
    public class BuildStepDependencySorterTests
    {
        [Test]
        public void Sort_ResolvesASimpleDepdency()
        {
            var buildSteps = new IBuildStep[]
                {
                    new BuildStepA(),
                    new BuildStepB()
                };

            var sut = new BuildStepDependencySorter();

            var result = sut.Sort(buildSteps).ToArray();

            result[0].Should().BeOfType<BuildStepB>();
            result[1].Should().BeOfType<BuildStepA>();

        }

        [Test]
        public void Sort_ThrowsCircularReferenceExceptionFor3StageCircularReference()
        {
            var buildSteps = new IBuildStep[]
                {
                    new Circular1(),
                    new Circular2(),
                    new Circular3()
                };

            var sut = new BuildStepDependencySorter();

            Assert.Throws<CircularDependencyException>(() => sut.Sort(buildSteps));

        }

        [Test]
        public void Sort_ThrowsCircularReferenceExceptionFor5StageCircularReference()
        {
            var buildSteps = new IBuildStep[]
                {
                    new CircularFiveStepA(), 
                    new CircularFiveStepB(), 
                    new CircularFiveStepC(), 
                    new CircularFiveStepD(), 
                    new CircularFiveStepE()
                };

            var sut = new BuildStepDependencySorter();

            Assert.Throws<CircularDependencyException>(() => sut.Sort(buildSteps));

        }

        [Test]
        public void Sort_ResolvesASequentialDepdency()
        {
            var buildSteps = new IBuildStep[]
                {
                    new SequentialDepdencyStepD(),
                    new SequentialDepdencyStepC(),
                    new SequentialDepdencyStepB(),
                    new SequentialDepdencyStepA()                    
                };

            var sut = new BuildStepDependencySorter();

            var result = sut.Sort(buildSteps).ToArray();

            result[0].Should().BeOfType<SequentialDepdencyStepA>();
            result[1].Should().BeOfType<SequentialDepdencyStepB>();
            result[2].Should().BeOfType<SequentialDepdencyStepC>();
            result[3].Should().BeOfType<SequentialDepdencyStepD>();

        }

        [Test]
        public void Sort_ResolvesMultipleDependencies()
        {
            var buildSteps = new IBuildStep[]
                {
                    new MultipleDepdencyStepC(), 
                    new MultipleDepdencyStepB(),
                    new MultipleDepdencyStepA()                                     
                };

            var sut = new BuildStepDependencySorter();

            var result = sut.Sort(buildSteps).ToArray();

            Assert.IsTrue((result[0].GetType() == typeof(MultipleDepdencyStepA) && result[1].GetType() == typeof(MultipleDepdencyStepB)) ||
                          (result[1].GetType() == typeof(MultipleDepdencyStepA) && result[0].GetType() == typeof(MultipleDepdencyStepB)));
            result[2].Should().BeOfType<MultipleDepdencyStepC>();

        }
    }
}
