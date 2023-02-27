-> start

=== start ===
Oh, hello! Elon told me about you! Welcome! 
*[Who are you?]
-> who_are_you

=== who_are_you ===
My name's Jesper. I am an engineer here on the station. My job is to ensure all systems work as intended. During my work I created some interesting things that may be useful to you. I can offer you them for a modest fee.
*[What do you want in return?]
-> fee
*[Can't I get them for free?]
-> deny

=== fee ===
I'm interested in little gold ingots from the Moon and Mars. If you find any - bring them to me and we will see what we can do. 
*[Ok]
    -> END
=== deny ===
I'am afraid not. You see, it's not that I`m not willing to help you, but I need something from that planets....for scientific purposes. See? We can help each other.
*[All right then. So what do you want in return?]
-> fee