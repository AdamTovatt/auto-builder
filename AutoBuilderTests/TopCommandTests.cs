using AutoBuilder.Models;

namespace AutoBuilderTests
{
    [TestClass]
    public class TopCommandTests
    {
        [TestMethod]
        public void TopCommandIsParsedCorrectly()
        {
            string topCommandString = @"top - 21:08:00 up 3 days, 21:19,  1 user,  load average: 0.00, 0.20, 0.62
                                        Tasks: 161 total,   1 running, 160 sleeping,   0 stopped,   0 zombie
                                        %Cpu(s):  3.4 us,  4.6 sy,  0.0 ni, 92.0 id,  0.0 wa,  0.0 hi,  0.0 si,  0.0 st
                                        MiB Mem :   7898.9 total,   3738.5 free,    336.6 used,   3823.9 buff/cache
                                        MiB Swap:    100.0 total,    100.0 free,      0.0 used.   7191.5 avail Mem

                                        PID USER      PR  NI    VIRT    RES    SHR S  %CPU  %MEM     TIME+ COMMAND
                                        1683 pi        20   0   11136   3072   2656 R  15.8   0.0   0:00.08 top -n 1 -b -u pi -c -w512
                                        1175 pi        20   0   14412   7368   6508 S   0.0   0.1   0:00.21 /lib/systemd/systemd --user
                                        1176 pi        20   0   38384   4100   1760 S   0.0   0.1   0:00.00 (sd-pam)
                                        1195 pi        20   0   14520   4540   3596 S   0.0   0.1   0:00.06 sshd: pi@pts/0
                                        1196 pi        20   0    8600   3924   2964 S   0.0   0.0   0:00.10 -bash
                                        13821 pi        20   0  732764  54732  35816 S   0.0   0.7   0:26.26 /home/pi/sakur/image-serializer-api/ImageSerializerApi/bin/Release/net5.0/linux-arm/publish/ImageSerializerApi
                                        15545 pi        20   0  811972 114448  60764 S   0.0   1.4   1926:51 /home/pi/sakur/betapet-bot-api/betapet-bot-api/bin/Release/net6.0/linux-arm/betapet-bot-api
                                        15626 pi        20   0  242632  46964  32824 S   0.0   0.6   0:25.66 /home/pi/sakur/betapet-bot-caller/betapet-bot-caller/bin/Release/net6.0/linux-arm/betapet-bot-caller --username DavidRdrgz --password gunnaral
                                        25803 pi        20   0  772952 115704  52232 S   0.0   1.4   1:11.80 /home/pi/sakur/room-reservation-api/RoomReservationApi/bin/Release/net5.0/linux-arm/publish/RoomReservationApi
                                        28836 pi        20   0  791032  98356  41948 S   0.0   1.2   1:13.39 /home/pi/sakur/auto-builder/auto-builder/bin/Release/net5.0/linux-arm/publish/auto-builder";

            TopCommand top = new TopCommand(topCommandString);

            Assert.IsTrue(top.LoadAverage1Minute == (0.00 / 4) * 100);
            Assert.IsTrue(top.LoadAverage5Minute == (0.20 / 4) * 100);
            Assert.IsTrue(top.LoadAverage15Minute == (0.62 / 4) * 100);
            Assert.IsTrue(top.ApplicatonRows.Count == 10);
        }
    }
}