package com.company.project.service.impl;

import com.company.project.dao.PlayerMapper;
import com.company.project.model.Player;
import com.company.project.service.PlayerService;
import com.company.project.core.AbstractService;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.annotation.Resource;


/**
 * Created by CodeGenerator on 2019/11/21.
 */
@Service
@Transactional
public class PlayerServiceImpl extends AbstractService<Player> implements PlayerService {
    @Resource
    private PlayerMapper playerMapper;

}
