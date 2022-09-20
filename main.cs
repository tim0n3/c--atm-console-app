using System;
using System.Diagnostics;

class BastionFirewall
{
    static void Main()
    {
        // lets say we want to run this command:    
        //  t=$(echo 'this is a test'); echo "$t" | grep -o 'is a'
        var output = ExecuteBashCommand(command: "t=$(echo 'this is a test'); echo \"$t\" | grep -o 'is a'");
        var output = ExecuteBashCommand(command: "echo 'prevent smurf attacks.'; echo 1 > /proc/sys/net/ipv4/icmp_echo_ignore_broadcasts; echo 0 > /proc/sys/net/ipv4/conf/all/accept_redirects; echo 'Drop source routed packets.'; echo 0 > /proc/sys/net/ipv4/conf/all/accept_source_route; echo 'prevent SYN Flood and TCP Starvation'; sysctl -w net/ipv4/tcp_syncookies=1; sysctl -w net/ipv4/tcp_timestamps=1; echo 2048 > /proc/sys/net/ipv4/tcp_max_syn_backlog; echo 3 > /proc/sys/net/ipv4/tcp_synack_retries; echo 'Address Spoofing Protection'; echo 1 > /proc/sys/net/ipv4/conf/all/rp_filter; echo 'Disable SYN Packet tracking'; sysctl -w net/netfilter/nf_conntrack_tcp_loose=0");

        // output the result
        Console.WriteLine(output);
    }

    static string ExecuteBashCommand(string command)
    {
        // according to: https://stackoverflow.com/a/15262019/637142
        // thanks to this we will pass everything as one command
        command = command.Replace("\"","\"\"");

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \""+ command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        proc.Start();
        proc.WaitForExit();

        return proc.StandardOutput.ReadToEnd();
    }
}