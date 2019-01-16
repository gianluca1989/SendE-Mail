# SendE-Mail

Questo programma semplicemente serve a mandare E-Mail. Le mail degli utenti saranno salvate su un database e, al momento 
dell'avvio del programma, viene letto il database e mandata una mail per ogni utente.

Ho cercato di fare in modo che il programma non si fermi mai. Gli unici casi in cui l'invio viene interrotto sono:

-   in caso in cui i dati del server, nel file di configurazione, siano errati.
-   in caso la mail del mittente sia errata, (viene controllato il formato della mail con una regular expression).
-   In caso il database non abbia nessun dato all'interno, (nel database sono presenti i dati del destinatario).

-   Viene invece bloccato l'invio di una mail in caso il formato della e-mail in esame non sia corretto, 
    (sempre controllato con una regular expression).

Vengono invece lanciati warning:

-   In caso i Nomi e le e-mail del Cc non corrispondano, (le e-mail vengono controllate se giuste o meno, e se giuste inviate; 
    i nomi eliminati e sostituiti con una stringa vuota, poiché potrebbero non corrispondere all'utente giusto).
-   in caso la lista del Cc sia vuota, ma comunque l'invio delle mail valide viene eseguito.
-   In caso la path degli allegati sia sbagliata (per esempio non contenga il nome di nessuna colonna della tabella su database). 
    In questo caso la parte errata viene eliminata dalla path.
-   In caso il file non sia trovato nel percorso, (viene scartato e non inviato).
-   in caso non sia stato possibile trovare il soggetto o il testo della mail. Anch'essi vengono semplicemente ignorati e la mail spedita.
