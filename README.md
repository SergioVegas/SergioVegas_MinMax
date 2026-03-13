
## Funcionament de l'algorisme

1. **Simulació de moviments:** Per a cada casella buida, la IA simula el seu moviment i crida recursivament la funció Minimax canviant el torn (de maximitzador a minimitzador).
2. **Avaluació d'estats finals:** Quan el joc arriba a un estat terminal (guanya algú o hi ha empat), s'assigna una puntuació:
   - **+10 punts:** Guanya la IA.
   - **-10 punts:** Guanya el jugador humà.
   - **0 punts:** Empat.
3. **Factor de profunditat:** Hem afegit un factor de profunditat  a les puntuacions. Això fa que la IA prefereixi guanyar en menys moviments o allargar la partida si sap que perdrà, buscant sempre l'eficiència.
4. **Presa de decisions:** El nivell que l'interesa maximitzar el valor, en aquest cas la IA tria el valor més alt retornat per les seves branques, mentre que el que vol minimitzar el valor, nosaltres, tria el més baix.

## Poda Alfa-Beta (Alpha-Beta Pruning)

Per optimitzar el rendiment i evitar el càlcul de branques innecessàries, hem implementat la Poda Alfa-Beta, aquesta millora permet descartar rutes de l'arbre de decisió que ja sabem que no seran triades pels jugadors.

### Com s'ha implementat:
Dins de la funció recursiva Minimax, s'han passat dos paràmetres addicionals: alpha i beta.

- **Alpha:** Representa la millor puntuació que el maximitzador té assegurada fins al moment.
- **Beta:** Representa la millor puntuació que el minimitzador té assegurada fins al moment.

**Lògica de la poda:**
Durant l'exploració, si en qualsevol moment el valor de **Beta** és menor o igual al de **Alpha** (beta <= alpha), s'executa un break. Això significa que el jugador actual ja ha trobat una opció millor en una altra branca, i per tant, no cal seguir explorant les opcions restants d'aquest node, estalviant així una gran quantitat de recursos computacionals.

