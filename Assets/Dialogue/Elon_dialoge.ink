-> start

=== start ===
Hello there! Name’s Elon! I’ll be your guide and fellow traveler in your journey! Listen, we need to retrieve some ancient artifacts from the Moon and Mars. We assume that those artifacts were left there by some other life form! They may provide us with some really valuable scientific data for further research. 
    *[So what's the matter?]
        -> what_is_the_matter
        
=== what_is_the_matter ===
The thing is, areas where those artifacts are located are full of dangers and wonders, so we need a brave volunteer to help us out. Now look at you, so daring, so… audacious! I can’t imagine a better candidate!  
    *[Just wrap it up, please]
        ->closing_story
    *[What are those artifacts, exactly?]
        ->details_then_close
        
        
=== closing_story ===
Anyway, here’s what we’re gonna do. First of all, you’ll need to learn some things before I can let you go on your own. When you’re ready, just approach the gateway in the main hall, and you will land on our beloved Earth, where your humble servant will be already waiting for you. Chao!
    *[Ok]
        -> END
    
=== details_then_close ===
Well, we don't know for sure yet. The say those may be some instruments or details left by another life form to establish contact with us. There is also a word that those are powerful ansient artifact that were lost long ago. Who knows....
    *[Aha..]
        -> closing_story
        