using NSubstitute;
using NUnit.Framework;
using System.Linq;
using FluentAssertions;

namespace BuilderFramework.Tests
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void Ctor_CreatesAnEmptySetOfBuildSteps()
        {
            // Arrange
            Builder sut;

            // Act
            sut = new Builder();

            // Assert
            sut.BuildSteps.Count().Should().Be(0);
        }

        [Test]
        public void With_AddsBuildStep()
        {
            // Arrange
            var buildStep = Substitute.For<IBuildStep>();
            var sut = new Builder();

            // Act
            sut.With(buildStep);

            // Assert
            sut.BuildSteps.Count().Should().Be(1);
        }

        public class StepBuilder
        {
            public IBuildStep Build()
            {
                return Substitute.For<IBuildStep>();
            }
        }

        [Test]
        public void WithGenericBuilder_AddsBuildStep()
        {
            
            // Arrange            
            var sut = new Builder();

            // Act
            sut.With<StepBuilder>(b => b.Build());

            // Assert
            sut.BuildSteps.Count().Should().Be(1);
            
        }



        [Test]
        public void Commit_CallsCommitOnEachBuildStepOnce()
        {
            // Arrange
            var buildSteps = new[]
                {
                    Substitute.For<IBuildStep>(),
                    Substitute.For<IBuildStep>(),
                    Substitute.For<IBuildStep>()
                };

            var sut = new Builder()
                        .With(buildSteps[1])
                        .With(buildSteps[2])
                        .With(buildSteps[0]);
            

            // Act
            sut.Commit();

            // Assert
            buildSteps[0].Received(1).Commit();
            buildSteps[1].Received(1).Commit();
            buildSteps[2].Received(1).Commit();
            
        }

        [Test]
        public void Rollback_CallsCommitOnEachBuildStepOnce()
        {
            // Arrange
            var buildSteps = new[]
                {
                    Substitute.For<IBuildStep>(),
                    Substitute.For<IBuildStep>(),
                    Substitute.For<IBuildStep>()
                };

            var sut = new Builder()
                        .With(buildSteps[1])
                        .With(buildSteps[2])
                        .With(buildSteps[0]);

            // Act
            sut.Rollback();

            // Assert
            buildSteps[0].Received(1).Rollback();
            buildSteps[1].Received(1).Rollback();
            buildSteps[2].Received(1).Rollback();

        }
    }
}
