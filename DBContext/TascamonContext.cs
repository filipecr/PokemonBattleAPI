using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TheProjectTascamon.Models;
using TheProjectTascamon.Service;

namespace TheProjectTascamon.DBContext;

public partial class TascamonContext : DbContext
{
    private readonly string _conn;
    public TascamonContext(string conn)
    {
        _conn = conn;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_conn);
        }
    }


    public TascamonContext(DbContextOptions<TascamonContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Battle> Battles { get; set; }

    public virtual DbSet<BattleLog> BattleLogs { get; set; }

    public virtual DbSet<BattleStatsLog> BattleStatsLogs { get; set; }

    public virtual DbSet<Move> Moves { get; set; }

    public virtual DbSet<MovesPokemon> MovesPokemons { get; set; }

    public virtual DbSet<Pokemon> Pokemons { get; set; }

    public virtual DbSet<TrainerPokemonMove> TrainerPokemonMoves { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersBattle> UsersBattles { get; set; }

    public virtual DbSet<UsersPokemon> UsersPokemons { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        modelBuilder.Entity<Battle>(entity =>
        {
            entity.HasKey(e => e.BattleId).HasName("PK__Battles__E3EE01973368C192");

            entity.Property(e => e.BattleId).HasMaxLength(50).HasColumnName("Battle_ID");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");

            entity.HasOne(d => d.WinnerNavigation).WithMany(p => p.Battles)
                .HasForeignKey(d => d.Winner)
                .HasConstraintName("FK__Battles__Winner__71D1E811");
        });

        modelBuilder.Entity<BattleLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Battle_log");

            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BattleId).HasMaxLength(50).HasColumnName("Battle_ID");
            entity.Property(e => e.CurrentPokemonId).HasColumnName("Current_PokemonID");
            entity.Property(e => e.PlayerId).HasColumnName("Player_ID");

            entity.HasOne(d => d.Battle).WithMany()
                .HasForeignKey(d => d.BattleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BattleSta__battl__73BA3083");

            entity.HasOne(d => d.CurrentPokemon).WithMany()
                .HasForeignKey(d => d.CurrentPokemonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BattleSta__Pokem__76969D2E");

            entity.HasOne(d => d.Player).WithMany()
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BattleSta__Playe__74AE54BC");
        });

        modelBuilder.Entity<BattleStatsLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Battle_stats_log");

            entity.Property(e => e.ApC)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("AP_c");
            entity.Property(e => e.BattleId).HasMaxLength(50).HasColumnName("Battle_ID");
            entity.Property(e => e.HpC)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("HP_c");
            entity.Property(e => e.MagicResC)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("MagicRes_c");
            entity.Property(e => e.PlayerId).HasColumnName("Player_ID");
            entity.Property(e => e.PokemonId).HasColumnName("Pokemon_ID");
            entity.Property(e => e.SpeedC)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Speed_c");
            entity.Property(e => e.StatusEffect)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.HasOne(d => d.Battle).WithMany()
                .HasForeignKey(d => d.BattleId)
                .HasConstraintName("FK__Battle_st__Battl__282DF8C2");

            entity.HasOne(d => d.Pokemon).WithMany()
                .HasForeignKey(d => d.PokemonId)
                .HasConstraintName("FK__Battle_st__Pokem__30C33EC3");
        });

        modelBuilder.Entity<Move>(entity =>
        {
            entity.HasKey(e => e.MovesId).HasName("PK__Moves__7BBCC19AC8703F46");

            entity.Property(e => e.MovesId).HasColumnName("Moves_ID");
            entity.Property(e => e.MoveApChange).HasColumnName("Move_AP_Change");
            entity.Property(e => e.MoveHpChange).HasColumnName("Move_HP_Change");
            entity.Property(e => e.MoveName)
                .HasMaxLength(50)
                .HasColumnName("Move_Name");
            entity.Property(e => e.MovePower).HasColumnName("Move_Power");
            entity.Property(e => e.MoveStatus)
                .HasMaxLength(50)
                .HasColumnName("Move_Status");
            entity.Property(e => e.MoveType)
                .HasMaxLength(50)
                .HasColumnName("Move_Type");
        });

        modelBuilder.Entity<MovesPokemon>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Moves_Pokemon");

            entity.Property(e => e.MovesId).HasColumnName("Moves_ID");
            entity.Property(e => e.PokemonId).HasColumnName("Pokemon_ID");

            entity.HasOne(d => d.Moves).WithMany()
                .HasForeignKey(d => d.MovesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Moves_Pok__Moves__5629CD9C");

            entity.HasOne(d => d.Pokemon).WithMany()
                .HasForeignKey(d => d.PokemonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Moves_Pok__Pokem__571DF1D5");
        });

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.ToTable("Pokemon");

            entity.Property(e => e.PokemonId).HasColumnName("Pokemon_ID");
            entity.Property(e => e.Ap).HasColumnName("AP");
            entity.Property(e => e.Hp).HasColumnName("HP");
            entity.Property(e => e.Speed).HasColumnName("Speed");
            entity.Property(e => e.PokemonName)
                .HasMaxLength(50)
                .HasColumnName("Pokemon_Name");
            entity.Property(e => e.PokemonType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Pokemon_Type");
        });

        modelBuilder.Entity<TrainerPokemonMove>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Trainer_Pokemon_Moves");

            entity.Property(e => e.MovesId).HasColumnName("Moves_ID");
            entity.Property(e => e.PokemonId).HasColumnName("Pokemon_ID");
            entity.Property(e => e.UsernameId).HasColumnName("Username_ID");

            entity.HasOne(d => d.Moves).WithMany()
                .HasForeignKey(d => d.MovesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trainer_P__Moves__5CD6CB2B");

            entity.HasOne(d => d.Pokemon).WithMany()
                .HasForeignKey(d => d.PokemonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trainer_P__Pokem__5DCAEF64");

            entity.HasOne(d => d.Username).WithMany()
                .HasForeignKey(d => d.UsernameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trainer_P__Usern__5EBF139D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__ED4DE4420F50DBA8");

            entity.Property(e => e.Id).HasColumnName("Users_ID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<UsersBattle>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Users_Battles");

            entity.Property(e => e.BattleId).HasColumnName("Battle_ID");
            entity.Property(e => e.UsersId).HasColumnName("Player_ID");
            entity.HasKey(ub => new { ub.UsersId, ub.BattleId });
            entity.HasOne(d => d.Battle).WithMany()
                .HasForeignKey(d => d.BattleId)
                .HasConstraintName("FK__Users_Bat__Battl__2EDAF651");

            entity.HasOne(d => d.Player).WithMany()
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("FK__Users_Bat__Playe__2FCF1A8A");
        });

        modelBuilder.Entity<UsersPokemon>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Users_Pokemon");

            entity.Property(e => e.PokemonId).HasColumnName("Pokemon_ID");
            entity.Property(e => e.UsersId).HasColumnName("Users_ID");

            entity.HasOne(d => d.Pokemon).WithMany()
                .HasForeignKey(d => d.PokemonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users_Pok__Pokem__52593CB8");

            entity.HasOne(d => d.Users).WithMany()
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users_Pok__Users__5165187F");
        });

        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
