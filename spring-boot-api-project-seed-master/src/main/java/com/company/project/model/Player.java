package com.company.project.model;

import javax.persistence.*;

public class Player {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    private String charactor;

    private String graveyard;

    private String hand;

    private String hiden;

    private String ondesk;

    private String user;

    /**
     * @return id
     */
    public Integer getId() {
        return id;
    }

    /**
     * @param id
     */
    public void setId(Integer id) {
        this.id = id;
    }

    /**
     * @return charactor
     */
    public String getCharactor() {
        return charactor;
    }

    /**
     * @param charactor
     */
    public void setCharactor(String charactor) {
        this.charactor = charactor;
    }

    /**
     * @return graveyard
     */
    public String getGraveyard() {
        return graveyard;
    }

    /**
     * @param graveyard
     */
    public void setGraveyard(String graveyard) {
        this.graveyard = graveyard;
    }

    /**
     * @return hand
     */
    public String getHand() {
        return hand;
    }

    /**
     * @param hand
     */
    public void setHand(String hand) {
        this.hand = hand;
    }

    /**
     * @return hiden
     */
    public String getHiden() {
        return hiden;
    }

    /**
     * @param hiden
     */
    public void setHiden(String hiden) {
        this.hiden = hiden;
    }

    /**
     * @return ondesk
     */
    public String getOndesk() {
        return ondesk;
    }

    /**
     * @param ondesk
     */
    public void setOndesk(String ondesk) {
        this.ondesk = ondesk;
    }

    /**
     * @return user
     */
    public String getUser() {
        return user;
    }

    /**
     * @param user
     */
    public void setUser(String user) {
        this.user = user;
    }
}