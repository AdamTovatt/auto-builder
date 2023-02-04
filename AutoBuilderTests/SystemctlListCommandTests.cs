using AutoBuilder.Models;

namespace AutoBuilderTests
{
    [TestClass]
    public class SystemctlListCommandTests
    {
        [TestMethod]
        public void ListCommandIsParsedCorrectly()
        {
            string commandString = @"   UNIT                               LOAD   ACTIVE SUB     DESCRIPTION
                                        alsa-restore.service               loaded active exited  Save/Restore Sound Card State
                                        auto-builder.service               loaded active running Auto Builder Api
                                        avahi-daemon.service               loaded active running Avahi mDNS/DNS-SD Stack
                                        betapet-bot-api.service            loaded active running Betapet bot api
                                        betapet-bot-caller.service         loaded active running Betapet bot caller service, will …
                                        bluetooth.service                  loaded active running Bluetooth service
                                        bthelper@hci0.service              loaded active exited  Raspberry Pi bluetooth helper
                                        console-setup.service              loaded active exited  Set console font and keymap
                                        cron.service                       loaded active running Regular background program proces…
                                        dbus.service                       loaded active running D-Bus System Message Bus
                                        dhcpcd.service                     loaded active running DHCP Client Daemon
                                        dphys-swapfile.service             loaded active exited  dphys-swapfile - set up, mount/un…
                                        fake-hwclock.service               loaded active exited  Restore / save the current clock
                                        getty@tty1.service                 loaded active running Getty on tty1
                                        hciuart.service                    loaded active running Configure Bluetooth Modems connec…
                                        ifupdown-pre.service               loaded active exited  Helper to synchronize boot up for…
                                        image-serializer-api.service       loaded active running Image Serializer Api
                                        keyboard-setup.service             loaded active exited  Set the console keyboard layout
                                        kmod-static-nodes.service          loaded active exited  Create list of static device node…
                                        ModemManager.service               loaded active running Modem Manager
                                        networking.service                 loaded active exited  Raise network interfaces
                                        nginx.service                      loaded active running A high performance web server and…
                                        polkit.service                     loaded active running Authorization Manager
                                        postgresql.service                 loaded active exited  PostgreSQL RDBMS
                                        postgresql@13-main.service         loaded active running PostgreSQL Cluster 13-main
                                        raspi-config.service               loaded active exited  LSB: Switch to ondemand cpu gover…
                                        rc-local.service                   loaded active exited  /etc/rc.local Compatibility
                                        rng-tools-debian.service           loaded active running LSB: rng-tools (Debian variant)
                                        room-reservation-api.service       loaded active running KTH Room Reservation Api
                                        rpi-eeprom-update.service          loaded active exited  Check for Raspberry Pi EEPROM upd…
                                        rsyslog.service                    loaded active running System Logging Service
                                        snapd.seeded.service               loaded active exited  Wait until snapd is fully seeded
                                        snapd.service                      loaded active running Snap Daemon
                                        ssh.service                        loaded active running OpenBSD Secure Shell server
                                        systemd-fsck-root.service          loaded active exited  File System Check on Root Device
                                        systemd-fsck@dev-disk-by\x2dpartu… loaded active exited  File System Check on /dev/disk/by…
                                        systemd-journal-flush.service      loaded active exited  Flush Journal to Persistent Stora…
                                        systemd-journald.service           loaded active running Journal Service
                                        systemd-logind.service             loaded active running User Login Management
                                        systemd-modules-load.service       loaded active exited  Load Kernel Modules
                                        systemd-random-seed.service        loaded active exited  Load/Save Random Seed
                                        systemd-remount-fs.service         loaded active exited  Remount Root and Kernel File Syst…
                                        systemd-sysctl.service             loaded active exited  Apply Kernel Variables
                                        systemd-sysusers.service           loaded active exited  Create System Users
                                        systemd-timesyncd.service          loaded active running Network Time Synchronization
                                        systemd-tmpfiles-setup-dev.service loaded active exited  Create Static Device Nodes in /dev
                                        systemd-tmpfiles-setup.service     loaded active exited  Create Volatile Files and Directo…
                                        systemd-udev-trigger.service       loaded active exited  Coldplug All udev Devices
                                        systemd-udevd.service              loaded active running Rule-based Manager for Device Eve…
                                        systemd-update-utmp.service        loaded active exited  Update UTMP about System Boot/Shu…
                                        systemd-user-sessions.service      loaded active exited  Permit User Sessions
                                        triggerhappy.service               loaded active running triggerhappy global hotkey daemon
                                        user-runtime-dir@1000.service      loaded active exited  User Runtime Directory /run/user/…
                                        user@1000.service                  loaded active running User Manager for UID 1000
                                        wpa_supplicant.service             loaded active running WPA supplicant

                                        LOAD   = Reflects whether the unit definition was properly loaded.
                                        ACTIVE = The high-level unit activation state, i.e. generalization of SUB.
                                        SUB    = The low-level unit activation state, values depend on unit type.
                                        55 loaded units listed. Pass --all to see loaded but inactive units, too.
                                        To show all installed unit files use 'systemctl list-unit-files'.
                                        ";

            SystemctlListCommand list = new SystemctlListCommand(commandString);

            Assert.IsTrue(list.ApplicationRows.Count() == 55);
            Assert.IsTrue(list.GetApplication("betapet-bot-api") != null);
        }
    }
}