package com.example.ooadproject.util;

import java.util.*;

public class TilePool {

    /*public static void main(String[] args) {
        List<Tile.Department> departments = new ArrayList<>();
        departments.add(Tile.Department.Math);
        departments.add(Tile.Department.Phy);
        departments.add(Tile.Department.Chem);
        departments.add(Tile.Department.Cse);
        TilePool tilePool = new TilePool(departments);
        System.out.println(tilePool.PoolToString());
    }*/

    private final int playerNum = 4;
    private List<Tile.Department> departments;
    private TileFactory tileFactory;
    private List<Tile> pool;
    private int next;
    private boolean shuffled;

    public int getSize() {
        return pool.size();
    }

    public int getRemain() {
        return pool.size() - next;
    }

    public TilePool(List<Tile.Department> departments) {
        if (departments.size() != playerNum) {
            try {
                throw new Exception("length should be 4");
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        this.departments = new ArrayList<>();
        this.tileFactory = new TileFactory();
        this.pool = new ArrayList<>();
        this.shuffled = false;
        this.next = 0;
        //System.out.println(Tile.Department.values().length);
        List<Tile.Department> all = new ArrayList<>(Arrays.asList(Tile.Department.values()));
        for (Tile.Department d : departments
        ) {

            if (!this.departments.contains(d)) {
                this.departments.add(d);
                all.remove(d);
            }
        }
        //System.out.println(this.departments.size());
        //System.out.println(all.size());
        if (this.departments.size() < 4) {
            Collections.shuffle(all);
        }
        while (this.departments.size() < 4) {
            this.departments.add(all.get(this.departments.size()));
        }
        this.generateNone();
        this.generateSpecial();


    }

    public String PoolToString() {
        String s = "";
        s += "TilePool with " + pool.size() + " Tiles\n";
        for (Tile t : this.pool
        ) {
            s += " - " + t.toString() + "\n";
        }
        return s;
    }

    public void Shuffle() {
        Collections.shuffle(this.pool);
        this.shuffled = true;
    }

    public int Draw(){
        if (!shuffled) {
            this.Shuffle();
        }
        if (this.pool.size() > this.next) {
            return pool.get(next++).getId();
        } else {
            return -1;
        }
    }

    private void generateSpecial() {
        this.generateKing();
        this.generateLogo();
        this.generateSign();
        this.generateLand();
    }

    private void generateKing() {
        int r = this.calcReserve(0, 0);
        for (int i = 0; i < 4; i++) {
            this.pool.add(this.tileFactory.getTile(
                    (Tile.Special.King.value() << Tile.Location.Special.value()) +
                            (i << Tile.Location.Unique.value()) +
                            r
            ));
        }
    }

    private void generateLogo() {
        int r = this.calcReserve(0, 0);
        for (int i = 0; i < 4; i++) {
            this.pool.add(this.tileFactory.getTile(
                    (Tile.Special.Logo.value() << Tile.Location.Special.value()) +
                            (i << Tile.Location.Unique.value()) +
                            r
            ));
        }
    }

    private void generateSign() {
        int r = this.calcReserve(0, 0);
        int i = 0;
        for (Tile.Department d : this.departments) {
            this.pool.add(this.tileFactory.getTile(
                    (Tile.Special.Sign.value() << Tile.Location.Special.value()) +
                            (d.value() << Tile.Location.Department.value()) +
                            r
            ));
        }
    }

    private void generateLand() {
        int r = this.calcReserve(0, 0);
        for (Tile.Land l: Tile.Land.values()
             ) {
            for (int i = 0; i < 4; i++) {
                this.pool.add(this.tileFactory.getTile(
                        (Tile.Special.Land.value() << Tile.Location.Special.value()) +
                                (l.value() << Tile.Location.Land.value())+
                                (i << Tile.Location.Unique.value()) +
                                r
                ));
            }
        }

    }

    private int calcReserve(int r0, int r1) {

        return (r0 << Tile.Location.Reserve0.value()) + (r1 << Tile.Location.Reserve1.value());
    }

    private void generateNone() {
        for (Tile.Department d : this.departments
        ) {
            for (int i = 1; i < 10; i++) {
                for (int j = 0; j < 4; j++) {
                    int temp = (
                            (Tile.Special.None.value() << Tile.Location.Special.value()) +
                                    (d.value() << Tile.Location.Department.value())+
                                    (i << Tile.Location.Sequence.value()) +
                                    (j << Tile.Location.Unique.value())
                    );
                    //System.out.println(temp);
                    this.pool.add(this.tileFactory.getTile(temp));
                }
            }
        }
    }


}
