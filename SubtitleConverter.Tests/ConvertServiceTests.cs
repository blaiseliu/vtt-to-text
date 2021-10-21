using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;
using SubtitleConverter.Services;

namespace SubtitleConverter.Tests
{
    public class ConvertServiceTests
    {
        private string vtt;

        [SetUp]
        public void Setup()
        {
            vtt = @"WEBVTT

00:00:00.000 --> 00:00:09.660 align:middle line:90%


00:00:09.660 --> 00:00:12.170 align:middle line:84%
KEVIN PORTTEUS: Equality
was the first and most

00:00:12.170 --> 00:00:14.780 align:middle line:84%
fundamental tenet
of the Founders'

00:00:14.780 --> 00:00:17.960 align:middle line:84%
understanding of the laws of
nature and of nature's god.

00:00:17.960 --> 00:00:19.400 align:middle line:84%
But they had a
very particular way

00:00:19.400 --> 00:00:22.430 align:middle line:84%
of understanding
what equality meant.

00:00:22.430 --> 00:00:25.460 align:middle line:84%
It did not mean
equality of outcome.

00:00:25.460 --> 00:00:30.110 align:middle line:84%
It didn't really even mean
equality of opportunity.

00:00:30.110 --> 00:00:32.630 align:middle line:84%
Equality only meant
that human beings

00:00:32.630 --> 00:00:37.700 align:middle line:84%
were equal in their rights and
duties under the natural law.

00:00:37.700 --> 00:00:41.750 align:middle line:84%
And that allowed for great
inequality in other respects.

00:00:41.750 --> 00:00:45.400 align:middle line:90%
[MUSIC PLAYING]

00:00:45.400 --> 00:00:50.000 align:middle line:90%";
        }

        [Test]
        public void RegexTest()
        {
            var output = Regex.Replace(vtt,@"\d\d:\d\d:\d\d.\d\d\d --> \d\d:\d\d:\d\d.\d\d\d align:middle line:\d\d%\n\n\n",
                "");
            var expected = @"WEBVTT\r\n\r\n00: 00:00.000-- > 00:00:09.660 align: middle line:90 %\r\n\r\n\r\n00: 00:09.660-- > 00:00:12.170 align: middle line:84 %\r\nKEVIN PORTTEUS: Equality\r\nwas the first and most\r\n\r\n00: 00:12.170-- > 00:00:14.780 align: middle line:84 %\r\nfundamental tenet\r\nof the Founders'\r\n\r\n00:00:14.780 --> 00:00:17.960 align:middle line:84%\r\nunderstanding of the laws of\r\nnature and of nature's god.\r\n\r\n00: 00:17.960-- > 00:00:19.400 align: middle line:84 %\r\nBut they had a\r\nvery particular way\r\n\r\n00: 00:19.400-- > 00:00:22.430 align: middle line:84 %\r\nof understanding\r\nwhat equality meant.\r\n\r\n00: 00:22.430-- > 00:00:25.460 align: middle line:84 %\r\nIt did not mean\r\nequality of outcome.\r\n\r\n00: 00:25.460-- > 00:00:30.110 align: middle line:84 %\r\nIt didn't really even mean\r\nequality of opportunity.\r\n\r\n00:00:30.110 --> 00:00:32.630 align:middle line:84%\r\nEquality only meant\r\nthat human beings\r\n\r\n00:00:32.630 --> 00:00:37.700 align:middle line:84%\r\nwere equal in their rights and\r\nduties under the natural law.\r\n\r\n00:00:37.700 --> 00:00:41.750 align:middle line:84%\r\nAnd that allowed for great\r\ninequality in other respects.\r\n\r\n00:00:41.750 --> 00:00:45.400 align:middle line:90%\r\n[MUSIC PLAYING]\r\n\r\n00:00:45.400 --> 00:00:50.000 align:middle line:90%";
            output.Should().Be(expected);
        }

        [Test]
        public void ServiceTest()
        {
            var service = new ConvertToTextServices();
            var output = service.ConvertVTT(vtt);
            var expected = @"WEBVTT   KEVIN PORTTEUS: Equality was the first and most  fundamental tenet of the Founders'  understanding of the laws of nature and of nature's god.  But they had a very particular way  of understanding what equality meant.  It did not mean equality of outcome.  It didn't really even mean equality of opportunity.  Equality only meant that human beings  were equal in their rights and duties under the natural law.  And that allowed for great inequality in other respects.  [MUSIC PLAYING] ";
            output.Should().Be(expected);
        }
    }
}