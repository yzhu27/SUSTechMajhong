package com.example.ooadproject.util;


public class Tile implements Comparable<Tile> {

    public static void main(String[] args) {
        System.out.println(Integer.toHexString(Special.None.value()));
        System.out.println(Integer.toHexString(Land.Hui.value()));
        System.out.println(Integer.toHexString(Department.Eee.value()));
    }

    public enum Special {
        //普通牌
        None(0xf),
        //癞子
        King(0x0),
        //校徽
        Logo(0x1),
        //签到
        Sign(0x2),
        //园
        Land(0x3);

        private int value;

        Special(int value) {
            this.value = value;
        }

        public int value() {
            return value;
        }


    }

    public enum Land {
        Xin(0x0),
        Lee(0x1),
        Hui(0x2),
        Cng(0x3),
        Zhi(0x4);

        private int value;

        Land(int value) {
            this.value = value;
        }

        public int value() {
            return value;
        }


    }

    public enum Department {
        Math(0x00),
        Phy(0x01),
        Chem(0x02),
        Bio(0x03),
        Ess(0x04),
        StatDs(0x05),
        Eee(0x20),
        Mse(0x21),
        Ocean(0x22),
        Cse(0x23),
        Ese(0x24),
        Mae(0x25),
        Mee(0x26),
        Bme(0x27),
        Sme(0x28),
        Sidm(0x29),
        Fin(0x40),
        Med(0x60);

        private int value;

        Department(int value) {
            this.value = value;
        }

        public int value() {
            return value;
        }


    }

    public enum Location {
        Unique(0),
        Reserve1(2),
        Sequence(4),
        Land(4),
        Department(8),
        Special(16),
        Reserve0(20),
        Zero(24);

        private int value;

        Location(int value) {
            this.value = value;
        }

        public int value() {
            return value;
        }


    }

    private int id;

    private boolean active;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public Tile(int id) {
        try {
            if (!IsCorrectID(id)) {
                throw new Exception("not a correct tile id " + "HEX:0x" + Integer.toHexString(id)+" INT:"+id);
            }
            this.id = id;
            //System.out.println("HEX:0x" + Integer.toHexString(id)+" INT:"+id);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public boolean equals(Object object) {
        return object != null && object instanceof Tile && ((Tile) object).getId() == this.id;
    }

    @Override
    public int hashCode() {
        return Integer.hashCode(this.id);
    }

    @Override
    public String toString() {
        switch (getSpecial(this.id)) {
            case King:
                //System.out.println("King");
                return Special.King + "(" + getUnique(this.id) + ")";
            case Logo:
                //System.out.println("Logo");
                return Special.Logo + "(" + getUnique(this.id) + ")";
            case Land:
                //System.out.println("Land");
                return getLand(this.id) + "(" + getUnique(this.id) + ")";
            case Sign:
                //System.out.println("Sign");
                return getDepartment(this.id) + "[" + Special.Sign + "](" + getUnique(this.id) + ")";
            case None:
                //System.out.println("None");
                return getDepartment(this.id) + "[" + getSeq(this.id) + "](" + getUnique(this.id) + ")";
            default:
                return "???";
        }
    }
    public static String IntToString(int id){
        switch (getSpecial(id)) {
            case King:
                //System.out.println("King");
                return Special.King + "(" + getUnique(id) + ")";
            case Logo:
                //System.out.println("Logo");
                return Special.Logo + "(" + getUnique(id) + ")";
            case Land:
                //System.out.println("Land");
                return getLand(id) + "(" + getUnique(id) + ")";
            case Sign:
                //System.out.println("Sign");
                return getDepartment(id) + "[" + Special.Sign + "](" + getUnique(id) + ")";
            case None:
                //System.out.println("None");
                return getDepartment(id) + "[" + getSeq(id) + "](" + getUnique(id) + ")";
            default:
                return "???";
        }
    }

    @Override
    public int compareTo(Tile o) {
        return this.id - o.getId();
    }

    public void Active() {
        this.active = true;
    }

    public void Unactive() {
        this.active = false;
    }

    public boolean IsActive() {
        return this.active;
    }

    public static int getUnique(int id) {
        return id & 0b11;
    }

    public static Department getDepartment(int id) {
        for (Department d : Department.values()
        ) {
            if (d.value() == ((id & 0xff00) >> Location.Department.value())) {
                //System.out.println(d);
                return d;
            }
        }
        return null;
    }

    public static Special getSpecial(int id) {
        for (Special s : Special.values()
        ) {
            if (s.value() == ((id & 0xf0000) >> Location.Special.value())) {
               // System.out.println(s);
                return s;
            }
        }
        return null;
    }

    public static int getSeq(int id) {
        return (id & 0xf0) >> Location.Sequence.value();
    }

    public static Land getLand(int id) {
        for (Land l : Land.values()
        ) {
            if (l.value() == getSeq(id)) {
                //System.out.println(l);
                return l;
            }
        }
        return null;
    }

    public boolean IsCorrectID(int id) {
        if (id >> Location.Zero.value() != 0) {
            //System.out.println(1);
            return false;
        }
        //System.out.println((id & 0xf0000) >> Location.Special.value());
        switch (getSpecial(id)) {
            case King:
                //System.out.println(2);
            case Logo:
                //System.out.println(3);
                return (id & 0xfff0) == 0;
            case Land:
                //System.out.println(4);
                return (id & 0xff00) == 0
                        &&
                        getLand((id & 0xf0) >> Location.Land.value()) != null;
            case Sign:
                //System.out.println(5);
            case None:
                //System.out.println(6);
                return getDepartment((id & 0xff00) >> Location.Department.value()) != null;
            default:
                System.out.println(7);
                return false;
        }
    }


}
