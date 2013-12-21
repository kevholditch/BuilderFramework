using System;
using System.Collections.Generic;
using BuilderFramework.Tests.TestTypes;
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
            sut = new Builder(Substitute.For<IBuildStepDependencySorter>());

            // Assert
            sut.BuildSteps.Count().Should().Be(0);
        }

        [Test]
        public void With_AddsBuildStep()
        {
            // Arrange
            var buildStep = Substitute.For<IBuildStep>();
            var sut = new Builder(Substitute.For<IBuildStepDependencySorter>());

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
            var sut = new Builder(Substitute.For<IBuildStepDependencySorter>());

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

            var dependencySorter = Substitute.For<IBuildStepDependencySorter>();
            dependencySorter.Sort(Arg.Any<IEnumerable<IBuildStep>>())
                            .Returns(buildSteps);

            var sut = new Builder(dependencySorter)
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

            var dependencySorter = Substitute.For<IBuildStepDependencySorter>();
            dependencySorter.Sort(Arg.Any<IEnumerable<IBuildStep>>())
                            .Returns(buildSteps);

            var sut = new Builder(dependencySorter)
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

        [Test]
        public void Commit_RunStepsInTheOrderTheDependencySorterDictates()
        {
            var results = new Queue<Tuple<Type, string>>();
            Action<Type, string> capture = (t, s) => 
                results.Enqueue(new Tuple<Type, string>(t, s));

            var buildSteps = new IBuildStep[]
                {
                    new Step1(capture), 
                    new Step2(capture), 
                };

            var dependencySorter = Substitute.For<IBuildStepDependencySorter>();
            dependencySorter.Sort(Arg.Any<IEnumerable<IBuildStep>>())
                            .Returns(buildSteps.Reverse());

            var sut = new Builder(dependencySorter)
                            .With(buildSteps[0])
                            .With(buildSteps[1]);
            sut.Commit();

            var firstStep = results.Dequeue();
            firstStep.Item1.Should().Be(typeof (Step2));
            firstStep.Item2.Should().Be("Commit");

            var secondStep = results.Dequeue();
            secondStep.Item1.Should().Be(typeof (Step1));
            secondStep.Item2.Should().Be("Commit");

            results.Count.Should().Be(0);

        }

        [Test]
        public void Rollback_RunStepsInTheReverseOrderOfDependencies()
        {
            var results = new Queue<Tuple<Type, string>>();
            Action<Type, string> capture = (t, s) =>
                results.Enqueue(new Tuple<Type, string>(t, s));

            var buildSteps = new IBuildStep[]
                {
                    new Step1(capture), 
                    new Step2(capture), 
                };

            var dependencySorter = Substitute.For<IBuildStepDependencySorter>();
            dependencySorter.Sort(Arg.Any<IEnumerable<IBuildStep>>())
                            .Returns(buildSteps.Reverse());

            var sut = new Builder(dependencySorter)
                            .With(buildSteps[0])
                            .With(buildSteps[1]);
            sut.Rollback();

            var firstStep = results.Dequeue();
            firstStep.Item1.Should().Be(typeof(Step1));
            firstStep.Item2.Should().Be("Rollback");

            var secondStep = results.Dequeue();
            secondStep.Item1.Should().Be(typeof(Step2));
            secondStep.Item2.Should().Be("Rollback");

            results.Count.Should().Be(0);

        }
    }
}
